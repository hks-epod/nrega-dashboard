Imports System
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
Partial Class nrega_reportdashboard_api_dashboard_report_yearly
    Inherits System.Web.UI.Page
    Dim con, con1 As New SqlConnection
    Dim cmd As New SqlCommand
    Dim myreader, myreader1 As SqlDataReader
    Public str, yr, cond, val_code, val_code_p, cond_p, state_code, short_name, tbl_fund As String
    Dim conobj As New ConnectNREGA
    Dim dsOrderRel, dsOrderRel1, dsOrderRel2, dsOrderRel3 As DataRelation
    Private _rows As Object

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
        If Request.QueryString("type") = "s" Then
            state_code = Request("state_code").ToString()
            val_code = "state_code"
            val_code_p = "Left(p.panchayat_code, 2)"
            cond_p = "LEFT(p.panchayat_code,2)='" & Request.QueryString("state_code") & "'"
            cond = "state_code='" & Request.QueryString("state_code") & "'"
            tbl_fund = "statefunds" & yr & ""
        ElseIf Request.QueryString("type") = "d" Then
            state_code = Left(Request("district_code").ToString(), 2)
            val_code = "district_code"
            val_code_p = "Left(p.panchayat_code, 4)"
            cond_p = "LEFT(p.panchayat_code,4)='" & Request.QueryString("district_code") & "'"
            cond = "district_code='" & Request.QueryString("district_code") & "'"
            tbl_fund = "demfunds" & yr & ""
        ElseIf Request.QueryString("type") = "b" Then
            state_code = Left(Request("block_code").ToString(), 2)
            val_code = "block_code"
            val_code_p = "Left(p.panchayat_code, 7)"
            cond_p = "LEFT(p.panchayat_code,7)='" & Request.QueryString("block_code") & "'"
            cond = "block_code='" & Request.QueryString("block_code") & "'"
            tbl_fund = "pofunds" & yr & ""
        End If

        Demand_registered_yearly(state_code, fin_year)

    End Sub
    Public Function Demand_registered_yearly(ByVal state_code As String, ByVal fin_year As String) As String

        yr = Mid(fin_year, 3, 2) & Mid(fin_year, 8, 2)
        Dim dt As New DataTable()
        Dim ds As New DataSet()
        Dim da As SqlDataAdapter
        con = conobj.connectCitizen(state_code)
        con.Open()
        Try
            str = "select short_name from states where state_code='" & state_code & "'"
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            short_name = cmd.ExecuteScalar

            '***********************************Demand_registered

            cmd = New SqlCommand("select p." & val_code & " code,isnull(SUM(isnull(reghh,0)),0)demand_register  from panchayats_rep" & yr & " p  left outer join demregister_panch" & yr & " pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & "", con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt0")
            cmd.Dispose()


            '***********************************Labour_Budget
            If Request.QueryString("type") = "b" Then
                cmd = New SqlCommand("select " & val_code & " code,isnull(SUM(isnull(manday_prj_curryear,0)),0)labour_budget from labour_budget_GP_trans" & yr & " where " & cond & " and month_code='03' group by " & val_code & "", con)
            Else
                cmd = New SqlCommand("select " & val_code & " code,isnull(SUM(isnull(manday_prj_curryear,0)),0)labour_budget from approved_labour_budget_GP_trans" & yr & " where " & cond & "  and month_code='03' group by " & val_code & "", con)
            End If
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt1")
            cmd.Dispose()

            '***********************************Work_Allotted / HHs completed 100 days

            cmd = New SqlCommand("select p." & val_code & " code,isnull(SUM(isnull(eproh,0)),0)work_alloted ,isnull(SUM(isnull(F100Days,0)),0) hh_completed_100days from panchayats_rep" & yr & " p  left outer join demregister_panch" & yr & " pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & "", con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt2")
            cmd.Dispose()

            '***********************************Unemployement_Allowance Duedays/DueAmount *****  Delayed Payment Duedays/DueAmount

            str = ""
            str = "select p." & val_code & " code, "
            str = str & " isnull(SUM(case when type='delay' then duedays end ),0)payable_days_delay,"
            str = str & " isnull(SUM(case when type='delay' then dueamt end ),0)payable_amount_delay,"
            str = str & "  isnull(SUM(case when type='unemp' then duedays end ),0)payable_days_unemp,"
            str = str & " isnull(SUM(case when type='unemp' then dueamt end ),0)payable_amount_unemp"
            str = str & "  from panchayats_rep" & yr & " p  left outer join nrega_unemp_allow" & yr & " pp on p.panchayat_code=pp.panchayat_code"
            str = str & "   where  p." & cond & " "
            str = str & " Group by p." & val_code & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt3")
            cmd.Dispose()

            '***********************************Unemployement_Allowance Approved_days/Rejected_days

            str = ""
            str = "select p." & val_code & " code, "
            str = str & " isnull(sum(case when bdo_po_approval='Y' then isnull(eligible_unemployement_days,0) end ),0)approved_days,  "
            str = str & " isnull(sum(case when bdo_po_approval='N' then isnull(eligible_unemployement_days,0) end ),0)rejected_days "
            str = str & "  from panchayats_rep" & yr & " p  left outer join approval_unemployment pp on p.panchayat_code=pp.panchayat_code"
            str = str & "   where p." & cond & " and financial_year='" & fin_year & "'"
            str = str & " Group by p." & val_code & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt4")
            cmd.Dispose()


            '***********************************Physical progress

            '***********************************PD(Generated)
            '*********************************** SC % / ST%  / Women%  /  HHs provided Employment / Average wage per PD/ Average Cost per PD 
            str = ""
            str = "select  p." & val_code & " code,"
            str = str & "    (case when (isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))>0"
            str = str & "   then (sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)) *100)/((isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))) else 0 end )SCper,"
            str = str & "   (case when (isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))>0"
            str = str & "   then (sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)) *100)/((isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))) else 0 end ) STper,"
            str = str & "   (case when (isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))>0"
            str = str & "   then (sum(isnull(cast(emp_gen_women as bigint),0)) *100)/((isnull(sum(isnull(cast(emp_gen_HH_SC_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_ST_pers as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_HH_OTH_pers as bigint),0)),0))) else 0 end ) Womenper,"
            str = str & "   (isnull(sum(isnull(cast(emp_gen_hh_sc as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_hh_st as bigint),0)),0)+isnull(sum(isnull(cast(emp_gen_hh_oth as bigint),0)),0)) hh_provided_employment,"
            str = str & "    round(isnull(case when (sum(wage_persondays))>0 then (sum(tot_wage_paid))/(sum(wage_persondays)) else 0 end,0),0)avg_wage_per_PD ,"
            str = str & "   round((case when (select isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)) from " & tbl_fund & " where " & cond & ")>0 then"
            str = str & "    sum(wage_persondays)/(select isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)) from " & tbl_fund & " where " & cond & ") else 0 end ),2)avg_cost_per_PD"
            str = str & "   from panchayats_rep" & yr & " p  left outer join misdemregister_panch" & yr & "  pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt5")
            cmd.Dispose()

            '*********************************** 3.	Work completion rate:
            '***********************************Demand_registered

            str = ""
            str = "select"
            str = str & "    (case when ( select count(*) as cnt  from wrkdtlcmp w     "
            str = str & "    left outer join " & short_name & "work_sanction b on w.work_code=b.work_code "
            str = str & "    and left(w.panchayat_code,7)=b.block_code       "
            str = str & "    inner join panchayats_rep" & yr & " p on p.panchayat_code=w.panchayat_code    "
            str = str & "    where    p." & cond & " and    "
            str = str & "     ( ( datediff(d,dateadd(m,est_mon_complete,w.Dt_Work_Start),getdate())>0   "
            str = str & "     and workstatus in ('03','04','05') )   "
            str = str & "     or (workstatus='05') ))>0 then "
            str = str & "     (   select count(*) as comp_cnt  from wrkdtlcmp w    "
            str = str & "    left outer join " & short_name & "work_sanction b on w.work_code=b.work_code and left(w.panchayat_code,7)=b.block_code"
            str = str & "    inner join panchayats_rep" & yr & " p on p.panchayat_code=w.panchayat_code   "
            str = str & "    where  (workstatus='05')  and p." & cond & ")*100         "
            str = str & "    /     "
            str = str & "    ( select count(*) as cnt  from wrkdtlcmp w     "
            str = str & "    left outer join " & short_name & "work_sanction b on w.work_code=b.work_code "
            str = str & "    and left(w.panchayat_code,7)=b.block_code       "
            str = str & "    inner join panchayats_rep" & yr & " p on p.panchayat_code=w.panchayat_code    "
            str = str & "     where    p." & cond & " and    "
            str = str & "     ( ( datediff(d,dateadd(m,est_mon_complete,w.Dt_Work_Start),getdate())>0   "
            str = str & "    and workstatus in ('03','04','05') )   "
            str = str & "    or (workstatus='05') )) else 0 end)	Work_completion_rate"
            cmd = New SqlCommand("select p." & val_code & " code,isnull(SUM(isnull(reghh,0)),0)demand_register  from panchayats_rep" & yr & " p  left outer join demregister_panch" & yr & " pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & "", con)
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt0")
            cmd.Dispose()

            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt6")
            cmd.Dispose()

            '*********************************** 5.	GPs with nil expenditure:
            str = ""
            str = "select "
            str = str & "  count(distinct case when (isnull(act_lab,0)+isnull(act_mat,0)+isnull(act_skilled,0)+isnull(contin,0)+isnull(n_contin,0))<=0  then p.panchayat_code  end ) NIL_EXP "
            str = str & "  from panchayats_rep" & yr & " a left outer join panfunds" & yr & " p on a.panchayat_code=p.panchayat_code where p." & cond & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt7")
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
            da.Fill(ds, "dt8")
            cmd.Dispose()

            '*********************************** 7.	GPs with no Approved Works
            str = ""
            str = "select count(*) as app_panch from panchayats where " & cond & " and urban is null  "
            str = str & "  and panchayat_code not in (select distinct panchayat_code from wrkexpnpanch" & yr & " where " & cond & " and status='02')"
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt9")
            cmd.Dispose()

            '*********************************** 8.	GPs with no ongoing Works 
            str = ""
            str = "select count(*) as ong_panch from panchayats where " & cond & " and urban is null "
            str = str & "  and panchayat_code not in (select distinct panchayat_code from wrkexpnpanch" & yr & " where " & cond & " and status='03')"
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt10")
            cmd.Dispose()

            '*********************************** Total Expenditure/ Wage%  /  Admin%           


            str = ""
            str = "select round(isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)),2) tot_exp,"
            str = str & " round((case when isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0))> 0"
            str = str & " then ((isnull(act_lab,0)*100)/(isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)))) else 0 end),2)wage_perc,"
            str = str & " round((case when isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0))> 0"
            str = str & " then ((isnull(contin,0)+isnull(n_contin,0))*100)/(isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0))) else 0 end ),2)adm_perc"
            str = str & "  from " & tbl_fund & " where " & cond & " "
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt11")
            cmd.Dispose()

            '*******************************************5.	eFMS:
            '*******************************************Total number of blocks/Started in No. of blocks (wages)/(Material)/(Admin):


            str = ""
            str = "select count(distinct b.block_code)total_blk,"
            str = str & " count(distinct case when flag='W' then f.block_code end)blk_wage_efms, "
            str = str & " count(distinct case when flag='M' then f.block_code end)blk_mat_efms,"
            str = str & " count(distinct case when flag='A' then f.block_code end)blk_adm_efms"
            str = str & "  from blocks_rep" & yr & " b left outer join fto_detail" & yr & " f with (nolock) "
            str = str & "  on b.block_code=f.block_code where b." & cond & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt12")

            '*******************************************% of expenditure through eFMS
            str = ""
            str = " select round((case when (select isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0)))>0 then"
            str = str & " ((select sum(amount)tot_exp from fto_detail" & yr & " fd with (nolock) "
            str = str & " inner join fto" & yr & " f with (nolock) on fd.fto_no=f.fto_no "
            str = str & " where  f." & cond & " and cr_flag='P' and fto_po_signed='Y')*100)/"
            str = str & " (select isnull(act_lab,0)+isnull((act_mat+act_skilled+act_tax),0)+(isnull(contin,0)+isnull(n_contin,0))) else 0 end),2) tot_exp_efms "
            str = str & " from " & tbl_fund & " where " & cond & ""
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt13")


            '*******************************************% of active workers A/Cs freezed /  % of Aadhaar seeding against total Active worker

            str = ""
            str = "select (case when isnull(SUM( case when active='y' then worker end),0)>0 then "
            str = str & "  (isnull(SUM( case when active='Y' and ac_freezed='Y' and modepay is not null then worker end),0) *100)/(isnull(SUM( case when active='y' then worker end),0)) else 0 end) frez_act_pers,"
            str = str & " (case when isnull(SUM( case when active='y' then worker end),0)>0 then "
            str = str & " (isnull(SUM( case when active='Y' and uid='Y' then worker end),0) *100)/(isnull(SUM( case when active='y' then worker end),0) )else 0 end ) aadhaar_seedpers"
            str = str & "  from panchayats_rep" & yr & " p inner join nrega_worker_detail w on"
            str = str & "     p.panchayat_code = w.panchayat_code"
            str = str & "  where active='Y' and p." & cond & " and"
            str = str & "  (Event_Flag is null or Event_Flag<>'D')"
            cmd = New SqlCommand(str, con)
            cmd.CommandTimeout = 0
            da = New SqlDataAdapter(cmd)
            da.Fill(ds, "dt14")
         

            con.Close()


        Catch ex As Exception
            Response.Write(ex.Message)
            Response.End()
        Finally
        End Try

        ''******************************************//////////////////////////////////////////////////////////////////////*********************************************************************
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))()

        Dim data As New Dictionary(Of String, Object)
        For Each table As DataTable In ds.Tables
            For Each dr1 As DataRow In table.Rows
                For Each col1 As DataColumn In table.Columns

                    If Not data.ContainsKey(col1.ColumnName) Then
                        data.Add(col1.ColumnName, dr1(col1))
                    End If

                Next
            Next
        Next
        rows.Add(data)

        Response.Write(serializer.Serialize(rows))
        Return serializer.Serialize(rows)


    End Function
End Class
