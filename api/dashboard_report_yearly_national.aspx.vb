﻿Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Web
Imports ConnectNREGA
Imports System.Web.Configuration
Imports System.Math
Imports System.Configuration
Imports System.IO
Imports System.Xml
Imports System.Web.Script.Serialization
Partial Class dashboard_report_yearly_national
    Inherits System.Web.UI.Page
    Dim con, con1 As New SqlConnection
    Dim cmd As New SqlCommand
    Dim myreader, myreader1 As SqlDataReader
    Public str, yr, cond, val_code, val_code_p, cond_p, state_code, short_name, tbl_fund As String
    Dim conobj As New ConnectNREGA
    Dim dsOrderRel, dsOrderRel1, dsOrderRel2, dsOrderRel3 As DataRelation
    Dim f_name, f_schema, code, path1, path2, source As String
    Dim fi, fj As FileInfo
    Private _rows As Object
    Public ds As New DataSet()

    Private Property rows(ByVal p1 As String, ByVal p2 As Integer) As Object
        Get
            Return _rows
        End Get
        Set(ByVal value As Object)
            _rows = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("fin_year") = "" Then
            Response.Write("Enter Fin Year")
            Response.End()

        End If

        Dim fin_year As String = Request("fin_year").ToString()
        yr = Mid(fin_year, 3, 2) & Mid(fin_year, 8, 2)

        f_name = "dashboard_report_yearly2_national_" + yr + ".xml"
        f_schema = "dashboard_report_yearly2_national_scheme_" + yr + ".xml"

        path1 = Server.MapPath("~\writereaddata\state_out\" + f_name)
        path2 = Server.MapPath("~\writereaddata\state_out\" + f_schema)
        fi = New FileInfo(path1)
        fj = New FileInfo(path2)

        If fi.Exists = True And fj.Exists = True Then
            source = "xml"
        End If


        If source = "xml" And DateDiff("d", fi.LastWriteTime.Date, Now.Date) = 0 Then
            Dim fs_schm As New FileStream(path2, FileMode.Open)
            ds.ReadXmlSchema(fs_schm)
            Dim fs_pa As New FileStream(path1, FileMode.Open)
            ds.ReadXml(fs_pa)
            fs_pa.Close()
            fs_schm.Close()
        Else
            Try


                str = "select state_name as name,state_code as code from states where state_code in ('02','00','36')  order by state_address5 ,state_name "
                '  str = "select state_name as name,state_code as code from states  order by state_address5 ,state_name "
                con = conobj.connectCitizen("24")
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
                cmd = New SqlCommand(str, con)
                myreader = cmd.ExecuteReader
                While myreader.Read
                    Demand_registered_yearly(myreader("code"), fin_year)
                End While
                myreader.Close()
                con.Close()


                Dim fouts As New FileStream(path2, FileMode.Create)
                ds.WriteXmlSchema(fouts)
                Dim fout As New FileStream(path1, FileMode.Create)
                ds.WriteXml(fout)
                fout.Close()
                fouts.Close()

            Catch ex As Exception
                Response.Write("<center><font color='red'>File is under process, please wait for some time.</font></center>")
                Response.End()
            Finally
                cmd.Dispose()
            End Try
        End If
        serialize_json()

    End Sub
    Public Sub Demand_registered_yearly(ByVal state_code As String, ByVal fin_year As String)

        yr = Mid(fin_year, 3, 2) & Mid(fin_year, 8, 2)
        Dim da As SqlDataAdapter

        state_code = state_code
        val_code = "state_code"
        val_code_p = "Left(p.panchayat_code, 2)"
        cond_p = "LEFT(p.panchayat_code,2)='" & state_code & "'"
        cond = "state_code='" & state_code & "'"
        tbl_fund = "statefunds" & yr & ""
        con = conobj.connectCitizen(state_code)
        con.Open()
        Try
            str = "select short_name from states where state_code='" & state_code & "'"
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            short_name = cmd.ExecuteScalar

            '***********************************Demand_registered

            cmd = New SqlCommand("select p." & val_code & " state_code,isnull(SUM(isnull(EDH,0)),0)demand_register  from panchayats_rep" & yr & " p  left outer join demregister_panch" & yr & " pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & "", con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()


            '***********************************Labour_Budget
            If Request.QueryString("type") = "b" Or Request.QueryString("type") = "p" Then
                cmd = New SqlCommand("select " & val_code & " state_code,isnull(SUM(isnull(manday_prj_curryear,0)),0)labour_budget from labour_budget_GP_trans" & yr & " where " & cond & " and month_code='03' group by " & val_code & "", con)
            Else
                cmd = New SqlCommand("select " & val_code & " state_code,isnull(SUM(isnull(manday_prj_curryear,0)),0)labour_budget from approved_labour_budget_GP_trans" & yr & " where " & cond & "  and month_code='03' group by " & val_code & "", con)
            End If
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '***********************************Work_Allotted / HHs completed 100 days

            cmd = New SqlCommand("select p." & val_code & " state_code,isnull(SUM(isnull(EofH,0)),0)work_alloted ,isnull(SUM(isnull(F100Days,0)),0) hh_completed_100days from panchayats_rep" & yr & " p  left outer join demregister_panch" & yr & " pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & "", con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '***********************************Unemployement_Allowance Duedays/DueAmount *****  Delayed Payment Duedays/DueAmount

            str = ""
            str = "select p." & val_code & " state_code, "
            str = str & " isnull(SUM(case when type='delay' then delay end ),0)payable_days_delay,"
            str = str & " isnull(SUM(case when type='delay' then dueamt end ),0)payable_amount_delay,"
            str = str & "  isnull(SUM(case when type='unemp' and p.state_code not in ('02','36') then duedays end ),0)payable_days_unemp,"
            str = str & " isnull(SUM(case when type='unemp' and p.state_code not in ('02','36') then dueamt end ),0)payable_amount_unemp"
            str = str & "  from panchayats_rep" & yr & " p  left outer join nrega_unemp_allow" & yr & " pp on p.panchayat_code=pp.panchayat_code"
            str = str & "   where  p." & cond & " "
            str = str & " Group by p." & val_code & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '***********************************Unemployement_Allowance Approved_days/Rejected_days

            str = ""
            str = "select p." & val_code & " state_code, "
            str = str & " isnull(sum(case when bdo_po_approval='Y' and p.state_code not in ('02','36') then isnull(eligible_unemployement_days,0) end ),0)approved_days,  "
            str = str & " isnull(sum(case when bdo_po_approval='N' and p.state_code not in ('02','36') then isnull(eligible_unemployement_days,0) end ),0)rejected_days "
            str = str & "  from panchayats_rep" & yr & " p  left outer join approval_unemployment pp on p.panchayat_code=pp.panchayat_code"
            str = str & "   where p." & cond & " and financial_year='" & fin_year & "'"
            str = str & " Group by p." & val_code & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()


            '***********************************Physical progress

            '***********************************PD(Generated)
            '*********************************** SC % / ST%  / Women%  /  HHs provided Employment / Average wage per PD/ Average Cost per PD 
            str = ""
            'str = "select  p." & val_code & " state_code,"
            'str = str & "   round( (case when (isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))>0"
            'str = str & "   then (sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)) *100)/((isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))) else 0 end ),2)SCper,"
            'str = str & "  round( (case when (isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))>0"
            'str = str & "   then (sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)) *100)/((isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))) else 0 end ),2) STper,"
            'str = str & "   round((case when (isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))>0"
            'str = str & "   then (sum(isnull(cast(emp_gen_women as bigint),0)) *100)/((isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))) else 0 end ),2) Womenper,"
            'str = str & "   (isnull(sum(isnull(cast(emp_gen_hh_sc as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_hh_st as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_hh_oth as bigint),0)),0)) hh_provided_employment,"
            'str = str & "    round(isnull(case when (sum(wage_persondays))>0 then (sum(tot_wage_paid))/(sum(wage_persondays)) else 0 end,0),0)avg_wage_per_PD ,"
            'str = str & "   round((case when (select isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)) from " & tbl_fund & " where " & cond & ")>0 then"
            'str = str & "    sum(wage_persondays)/(select isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)) from " & tbl_fund & " where " & cond & ") else 0 end ),2)avg_cost_per_PD"
            'str = str & "   from panchayats_rep" & yr & " p  left outer join misdemregister_panch" & yr & "  pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & ""


            str = "select  p." & val_code & " state_code, isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)sc, isnull(sum(isnull(cast(emp_gen_women as bigint),0)),0)wom,"
            str = str & "isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)st,isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+"
            str = str & " isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0)tot_pers,"
            str = str & "   (isnull(sum(isnull(cast(emp_gen_hh_sc as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_hh_st as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_hh_oth as bigint),0)),0)) hh_provided_employment,"
            str = str & "    sum(tot_wage_paid)wage_paid,sum(wage_persondays)wage_pers "
            str = str & "   from panchayats_rep" & yr & " p  left outer join misdemregister_panch" & yr & "  pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & ""


            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()


            '*********************************** 3.	Work completion rate:
            '***********************************Demand_registered

            str = ""
            'str = "select"
            'str = str & "    round((case when ( select count(*) as cnt  from wrkdtlcmp w     "
            'str = str & "    left outer join " & short_name & "work_sanction b on w.work_code=b.work_code "
            'str = str & "    and left(w.panchayat_code,7)=b.block_code       "
            'str = str & "    inner join panchayats_rep" & yr & " p on p.panchayat_code=w.panchayat_code    "
            'str = str & "    where    p." & cond & " and    "
            'str = str & "     ( ( datediff(d,dateadd(m,est_mon_complete,w.Dt_Work_Start),getdate())>0   "
            'str = str & "     and workstatus in ('03','04','05') )   "
            'str = str & "     or (workstatus='05') ))>0 then "
            'str = str & "     (   select count(*) as comp_cnt  from wrkdtlcmp w    "
            'str = str & "    left outer join " & short_name & "work_sanction b on w.work_code=b.work_code and left(w.panchayat_code,7)=b.block_code"
            'str = str & "    inner join panchayats_rep" & yr & " p on p.panchayat_code=w.panchayat_code   "
            'str = str & "    where  (workstatus='05')  and p." & cond & ")*100         "
            'str = str & "    /     "
            'str = str & "    ( select count(*) as cnt  from wrkdtlcmp w     "
            'str = str & "    left outer join " & short_name & "work_sanction b on w.work_code=b.work_code "
            'str = str & "    and left(w.panchayat_code,7)=b.block_code       "
            'str = str & "    inner join panchayats_rep" & yr & " p on p.panchayat_code=w.panchayat_code    "
            'str = str & "     where    p." & cond & " and    "
            'str = str & "     ( ( datediff(d,dateadd(m,est_mon_complete,w.Dt_Work_Start),getdate())>0   "
            'str = str & "    and workstatus in ('03','04','05') )   "
            'str = str & "    or (workstatus='05') )) else 0 end),2)	Work_completion_rate"

       
            str = "     select count(case when workstatus='05' then 1 end) as comp_cnt ,"
            str = str & "   count(case when ( datediff(d,dateadd(m,est_mon_complete,w.Dt_Work_Start),getdate())>0 and workstatus in ('03','04','05') )  or (workstatus='05')  then 1 end)as cnt"
            str = str & "  from wrkdtlcmp w    "
            str = str & "    left outer join " & short_name & "work_sanction b on w.work_code=b.work_code and left(w.panchayat_code,7)=b.block_code"
            str = str & "    inner join panchayats_rep" & yr & " p on p.panchayat_code=w.panchayat_code   "
            str = str & "    where  p." & cond & "    and  dbo.fiscalyear(w.Dt_Work_Start)='" & Request.QueryString("fin_year") & "' "

            'str = str & "    ( select count(*) as cnt  from wrkdtlcmp w     "
            'str = str & "    left outer join " & short_name & "work_sanction b on w.work_code=b.work_code "
            'str = str & "    and left(w.panchayat_code,7)=b.block_code       "
            'str = str & "    inner join panchayats_rep" & yr & " p on p.panchayat_code=w.panchayat_code    "
            'str = str & "     where    p." & cond & " and    "
            'str = str & "     ( ( datediff(d,dateadd(m,est_mon_complete,w.Dt_Work_Start),getdate())>0   "
            'str = str & "    and workstatus in ('03','04','05') )   "
            'str = str & "    or (workstatus='05') )) else 0 end),2)	Work_completion_rate"




            cmd = New SqlCommand("select p." & val_code & " state_code,isnull(SUM(isnull(reghh,0)),0)demand_register  from panchayats_rep" & yr & " p  left outer join demregister_panch" & yr & " pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & "", con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt0")
            cmd.Dispose()

            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '*********************************** 5.	GPs with nil expenditure:
            str = ""
            str = "select "
            str = str & "  count(distinct case when (isnull(act_lab,0)+isnull(act_mat,0)+isnull(act_skilled,0)+isnull(contin,0)+isnull(n_contin,0))<=0  then p.panchayat_code  end ) NIL_EXP "
            str = str & "  from panchayats_rep" & yr & " a left outer join panfunds" & yr & " p on a.panchayat_code=p.panchayat_code where p." & cond & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '*********************************** 6.	Total No. of GP's / GPs with no employment generation in last month:
            str = ""
            Dim mon_var As String = ""
            Select Case Now.Month - 1
                Case "01"
                    mon_var = "janreg"
                Case "02"
                    mon_var = "febreg"
                Case "03"
                    mon_var = "marchreg"
                Case "04"
                    mon_var = "aprilreg"
                Case "05"
                    mon_var = "mayreg"
                Case "06"
                    mon_var = "junereg"
                Case "07"
                    mon_var = "julyreg"
                Case "08"
                    mon_var = "augreg"
                Case "09"
                    mon_var = "sepreg"
                Case "10"
                    mon_var = "octreg"
                Case "11"
                    mon_var = "novreg"
                Case "12"
                    mon_var = "decreg"
            End Select

            str = "select  count(distinct p.panchayat_code )Tgp,count(case when isnull(" & mon_var & ",0)=0 then p.panchayat_code end) lastmon_nilgp from panchayats_rep" & yr & " p"
            str = str & "  left outer join  "
            str = str & "  mon_Wise_empprov" & yr & " m on m.panchayat_code=p.panchayat_code"
            str = str & "  where p." & cond & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '*********************************** 7.	GPs with no Approved Works

            str = ""
            str = "select count(*) as app_panch from panchayats where " & cond & " and urban is null  "
            str = str & "  and panchayat_code not in (select distinct panchayat_code from wrkexpnpanch" & yr & " where " & cond & " and status='02')"
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '*********************************** 8.	GPs with no ongoing Works 
            str = ""
            str = "select count(*) as ong_panch from panchayats where " & cond & " and urban is null "
            str = str & "  and panchayat_code not in (select distinct panchayat_code from wrkexpnpanch" & yr & " where " & cond & " and status='03')"
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '*********************************** Total Expenditure/ Wage%  /  Admin%           


            str = ""
            'str = "select round(isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)),2) tot_exp,"
            'str = str & " round((case when isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0))> 0"
            'str = str & " then ((isnull(act_lab,0)*100)/(isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)))) else 0 end),2)wage_perc,"
            'str = str & " round((case when isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0))> 0"
            'str = str & " then ((isnull(contin,0)+isnull(n_contin,0))*100)/(isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0))) else 0 end ),2)adm_perc"
            'str = str & "  from " & tbl_fund & " where " & cond & " "



            str = "select round(isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)),2) tot_exp,"
            str = str & " isnull(act_lab,0)wage,"
            str = str & " isnull(contin,0)+isnull(n_contin,0)adm"
            str = str & "  from " & tbl_fund & " where " & cond & " "

            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Dispose()

            '*******************************************5.	eFMS:
            '*******************************************Total number of blocks/Started in No. of blocks (wages)/(Material)/(Admin):


            'str = ""
            'str = "select count(distinct b.block_code)total_blk,"
            'str = str & " count(distinct case when flag='W' then f.block_code end)blk_wage_efms, "
            'str = str & " count(distinct case when flag='M' then f.block_code end)blk_mat_efms,"
            'str = str & " count(distinct case when flag='A' then f.block_code end)blk_adm_efms"
            'str = str & "  from blocks_rep" & yr & " b left outer join fto_detail" & yr & " f with (nolock) "
            'str = str & "  on b.block_code=f.block_code where b." & cond & ""
            'cmd = New SqlCommand(str, con)
            'cmd.CommandTimeout = 0
            'da = New SqlDataAdapter(cmd)
            'da.Fill(ds, "dt12")

            '*******************************************% of expenditure through eFMS
            'str = ""
            'str = " select round((case when (select isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)))>0 then"
            'str = str & " ((select sum(amount)tot_exp from fto_detail" & yr & " fd with (nolock) "
            'str = str & " inner join fto" & yr & " f with (nolock) on fd.fto_no=f.fto_no "
            'str = str & " where  f." & cond & " and cr_flag='P' and fto_po_signed='Y')*100)/"
            'str = str & " (select isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0))) else 0 end),2) tot_exp_efms "
            'str = str & " from " & tbl_fund & " where " & cond & ""
            'cmd = New SqlCommand(str, con)
            'cmd.CommandTimeout = 0
            'da = New SqlDataAdapter(cmd)
            'da.Fill(ds, "dt13")


            '*******************************************% of active workers A/Cs freezed /  % of Aadhaar seeding against total Active worker

            str = ""
            'str = "select round((case when isnull(SUM( case when active='y' then worker end),0)>0 then "
            'str = str & "  (isnull(SUM( case when active='Y' and ac_freezed='Y' and modepay is not null then worker end),0) *100)/(isnull(SUM( case when active='y' then worker end),0)) else 0 end),2) frez_act_pers,"
            'str = str & " round((case when isnull(SUM( case when active='y' then worker end),0)>0 then "
            'str = str & " (isnull(SUM( case when active='Y' and uid='Y' then worker end),0) *100)/(isnull(SUM( case when active='y' then worker end),0) )else 0 end ),2) aadhaar_seedpers"
            'str = str & "  from panchayats_rep" & yr & " p inner join nrega_worker_detail w on"
            'str = str & "     p.panchayat_code = w.panchayat_code"
            'str = str & "  where active='Y' and p." & cond & " and"
            'str = str & "  (Event_Flag is null or Event_Flag<>'D')"



            str = "select  isnull(SUM( case when active='y' then worker end),0)tot_act,"
            str = str & "  isnull(SUM( case when active='Y' and ac_freezed='Y' and modepay is not null then worker end),0)frez_act,"
            str = str & " isnull(SUM( case when active='Y' and uid='Y' then worker end),0) aadhaar_seed"
            str = str & "  from panchayats_rep" & yr & " p inner join nrega_worker_detail w on"
            str = str & "     p.panchayat_code = w.panchayat_code"
            str = str & "  where active='Y' and p." & cond & " and"
            str = str & "  (Event_Flag is null or Event_Flag<>'D')"

            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds)


            con.Close()


        Catch ex As Exception
            Response.Write(ex.Message)
            Response.End()
        Finally
        End Try

        ''******************************************//////////////////////////////////////////////////////////////////////*********************************************************************


    End Sub


    Public Function serialize_json() As String
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))()

        Dim data As New Dictionary(Of String, Object)
        For Each table As DataTable In ds.Tables
            ' For Each dr1 As DataRow In table.Rows
            Dim dd As Double = 0
            Dim i As Integer
            For Each col1 As DataColumn In table.Columns
                If col1.ColumnName <> "state_code" Then
                    For Each dr2 As DataRow In table.Rows()
                        If dr2(col1).ToString = "" Then
                            dr2(col1) = 0
                        End If
                        If dr2(col1).ToString <> "" Then
                            dd = dd + dr2(col1)
                        End If
                        'dr2(col1) = 0
                    Next
                End If
                If Not data.ContainsKey(col1.ColumnName) Then
                    data.Add(col1.ColumnName, Round(Convert.ToDecimal(dd), 2))
                End If
                dd = 0
            Next
            ' Next

        Next
        If data("tot_pers") > 0 Then
            data.Add("SCper", Round(Convert.ToDecimal(data("sc") * 100 / data("tot_pers")), 2))
            data.Add("STper", Round(Convert.ToDecimal(data("st") * 100 / data("tot_pers")), 2))
            data.Add("Womenper", Round(Convert.ToDecimal(data("wom") * 100 / data("tot_pers")), 2))
        End If
        If data("cnt") > 0 Then
            data.Add("Work_completion_rate", Round(Convert.ToDecimal(data("comp_cnt") * 100 / data("cnt")), 2))
        End If
        If data("tot_exp") > 0 Then
            data.Add("wage_perc", Round(Convert.ToDecimal(data("wage") * 100 / data("tot_exp")), 2))
            data.Add("adm_perc", Round(Convert.ToDecimal(data("adm") * 100 / data("tot_exp")), 2))
        End If
        If data("tot_act") > 0 Then
            data.Add("frez_act_pers", Round(Convert.ToDecimal(data("frez_act") * 100 / data("tot_act")), 2))
            data.Add("aadhaar_seedpers", Round(Convert.ToDecimal(data("aadhaar_seed") * 100 / data("tot_act")), 2))
        End If
        If data("wage_pers") > 0 Then
            data.Add("avg_wage_per_PD", Round(Convert.ToDecimal(data("wage_paid") / data("wage_pers")), 2))
        End If
        rows.Add(data)

        Response.Write(serializer.Serialize(rows))
        Return serializer.Serialize(rows)
    End Function
End Class
