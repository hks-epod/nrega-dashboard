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
Partial Class dashboard_report_national
    Inherits System.Web.UI.Page
    Dim con, con1 As New SqlConnection
    Dim cmd As New SqlCommand
    Dim myreader, myreader1 As SqlDataReader
    Public str, yr, cond, val_code, val_code_p, cond_p, state_code, pre_yr As String
    Dim conobj As New ConnectNREGA
    Public file1, fin_year, curr_finyear, mon As String
    Dim f_name, f_schema, code, path1, path2, source As String
    Dim fi, fj As FileInfo
    Public ds As New DataSet()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("fin_year") = "" Then
            Response.Write("Enter fin Year")
            Response.End()

        End If
        Dim fin_year As String = Request("fin_year").ToString()
        yr = Mid(fin_year, 3, 2) & Mid(fin_year, 8, 2)

        f_name = "dashboard_report_national_" + yr + ".xml"
        f_schema = "dashboard_report_national_scheme_" + yr + ".xml"

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


                str = "select state_name as name,state_code as code from states where state_code in ('17','30')  order by state_address5 ,state_name "
                ' str = "select state_name as name,state_code as code from states   order by state_address5 ,state_name "
                con = conobj.connectCitizen("24")
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
                cmd = New SqlCommand(str, con)
                myreader = cmd.ExecuteReader
                While myreader.Read
                    Demand_registered_monthly(myreader("code"), fin_year)
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
    Public Sub Demand_registered_monthly(ByVal state_code As String, ByVal fin_year As String)


        yr = Mid(fin_year, 3, 2) & Mid(fin_year, 8, 2)
        pre_yr = Mid(fin_year, 3, 2) - 1 & Mid(fin_year, 8, 2) - 1
        'Dim dt As New DataTable()
        Dim da As SqlDataAdapter


        state_code = state_code
        val_code = "state_code"
        val_code_p = "Left(p.panchayat_code, 2)"
        cond_p = "LEFT(p.panchayat_code,2)='" & state_code & "'"
        cond = "state_code='" & state_code & "'"


        con = conobj.connectCitizen(state_code)
        con.Open()

        '**************************************************Demand_registered

        If Month(Date.Now) > 3 Then
            curr_finyear = Year(Date.Now) & "-" & Year(Date.Now) + 1
        Else
            curr_finyear = Year(Date.Now) - 1 & "-" & Year(Date.Now)
        End If
        If HttpContext.Current.Request("fin_year") = curr_finyear Then
            mon = Month(Date.Now)
        Else
            mon = "15"
        End If
        If mon >= 1 And mon < 4 Then
            mon = mon + 9
        Else
            mon = mon - 3
        End If

        str = "select  p." & val_code & " state_code"
        str = str & "    , isnull(SUM(isnull(aprilreg,0)),0)april_demand_reg "
        If mon >= 2 Then
            str = str & "   , isnull(SUM(isnull(mayreg,0)),0)may_demand_reg"
        End If
        If mon >= 3 Then
            str = str & "    ,  isnull(SUM(isnull(junereg,0)),0)june_demand_reg"
        End If
        If mon >= 4 Then
            str = str & "    , isnull(SUM(isnull(julyreg,0)),0)july_demand_reg"
        End If
        If mon >= 5 Then
            str = str & "     , isnull(SUM(isnull(augreg,0)),0)aug_demand_reg"
        End If
        If mon >= 6 Then
            str = str & "    , isnull(SUM(isnull(sepreg,0)),0)sep_demand_reg "
        End If
        If mon >= 7 Then
            str = str & "     , isnull(SUM(isnull(octreg,0)),0)oct_demand_reg"
        End If
        If mon >= 8 Then
            str = str & "     ,isnull(SUM(isnull(novreg,0)),0)nov_demand_reg"
        End If
        If mon >= 9 Then
            str = str & "     , isnull(SUM(isnull(decreg,0)),0)dec_demand_reg"
        End If
        If mon >= 10 Then
            str = str & "     , isnull(SUM(isnull(janreg,0)),0)jan_demand_reg "
        End If
        If mon >= 11 Then
            str = str & "     , isnull(SUM(isnull(febreg,0)),0)feb_demand_reg "
        End If
        If mon >= 12 Then
            str = str & "     ,isnull(SUM(isnull(marchreg,0)),0)march_demand_reg"
        End If
        str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_dmd" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""
        '***********************************Labour_Budget

        str = "select " & val_code & " state_code ,isnull(SUM(case when month_code='04' then  isnull(manday_prj_curryear,0) end),0) april_lb, "
        str = str & "  isnull(SUM(case when month_code='05' then  isnull(manday_prj_curryear,0) end),0) may_lb ,"
        str = str & "  isnull(SUM(case when month_code='06' then  isnull(manday_prj_curryear,0) end),0) june_lb ,"
        str = str & "  isnull(SUM(case when month_code='07' then  isnull(manday_prj_curryear,0) end),0) july_lb ,"
        str = str & "  isnull(SUM(case when month_code='08' then  isnull(manday_prj_curryear,0) end),0) aug_lb, "
        str = str & "  isnull(SUM(case when month_code='09' then  isnull(manday_prj_curryear,0) end),0) sep_lb ,"
        str = str & "  isnull(SUM(case when month_code='10' then  isnull(manday_prj_curryear,0) end),0) oct_lb ,"
        str = str & "  isnull(SUM(case when month_code='11' then  isnull(manday_prj_curryear,0) end),0) nov_lb ,"
        str = str & "  isnull(SUM(case when month_code='12' then  isnull(manday_prj_curryear,0) end),0) dec_lb, "
        str = str & "  isnull(SUM(case when month_code='01' then  isnull(manday_prj_curryear,0) end),0) jan_lb ,"
        str = str & "  isnull(SUM(case when month_code='02' then  isnull(manday_prj_curryear,0) end),0) feb_lb ,"
        str = str & "  isnull(SUM(case when month_code='03' then  isnull(manday_prj_curryear,0) end),0) march_lb "
        If Request.QueryString("type") = "b" Then
            str = str & "   from labour_budget_GP_trans" & yr & " "
        Else
            str = str & "   from approved_labour_budget_GP_trans" & yr & " "
        End If
        str = str & "   where " & cond & " group by " & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""


        '***********************************Work_Allotted *********** HH provided employement

        str = "select p." & val_code & " state_code ,"
        str = str & "     isnull(SUM(isnull(aprilAPP,0)),0)april_work_allot,isnull(SUM(isnull(aprilreg,0)),0)april_hh_P_emp"
        If mon >= 2 Then
            str = str & "     , isnull(SUM(isnull(mayAPP,0)),0)may_work_allot,isnull(SUM(isnull(mayreg,0)),0)may_hh_P_emp"
        End If
        If mon >= 3 Then
            str = str & "    , isnull(SUM(isnull(juneAPP,0)),0)june_work_allot, isnull(SUM(isnull(junereg,0)),0)june_hh_P_emp "
        End If
        If mon >= 4 Then
            str = str & "    ,isnull(SUM(isnull(julyAPP,0)),0)july_work_allot, isnull(SUM(isnull(julyreg,0)),0)july_hh_P_emp"
        End If
        If mon >= 5 Then
            str = str & "     , isnull(SUM(isnull(augAPP,0)),0)aug_work_allot, isnull(SUM(isnull(augreg,0)),0)aug_hh_P_emp "
        End If
        If mon >= 6 Then
            str = str & "        ,  isnull(SUM(isnull(sepAPP,0)),0)sep_work_allot, isnull(SUM(isnull(sepreg,0)),0)sep_hh_P_emp "
        End If
        If mon >= 7 Then
            str = str & "     , isnull(SUM(isnull(octAPP,0)),0)oct_work_allot,  isnull(SUM(isnull(octreg,0)),0)oct_hh_P_emp"
        End If
        If mon >= 8 Then
            str = str & "    ,isnull(SUM(isnull(novAPP,0)),0)nov_work_allot,isnull(SUM(isnull(novreg,0)),0)nov_hh_P_emp"
        End If
        If mon >= 9 Then
            str = str & "    ,  isnull(SUM(isnull(decAPP,0)),0)dec_work_allot, isnull(SUM(isnull(decreg,0)),0)dec_hh_P_emp"
        End If
        If mon >= 10 Then
            str = str & "     ,  isnull(SUM(isnull(janAPP,0)),0)jan_work_allot ,  isnull(SUM(isnull(janreg,0)),0)jan_hh_P_emp"
        End If
        If mon >= 11 Then
            str = str & "    , isnull(SUM(isnull(febAPP,0)),0)feb_work_allot, isnull(SUM(isnull(febreg,0)),0)feb_hh_P_emp "
        End If
        If mon >= 12 Then
            str = str & "     ,isnull(SUM(isnull(marchAPP,0)),0)march_work_allot , isnull(SUM(isnull(marchreg,0)),0)march_hh_P_emp "
        End If
        str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_empprov" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""


        '***********************************HH provided employement

        'str = "select " & val_code_p & " state_code "
        'str = str & "     ,isnull(SUM(isnull(aprilreg,0)),0)april_hh_P_emp"
        'If mon >= 2 Then
        '    str = str & "     ,isnull(SUM(isnull(mayreg,0)),0)may_hh_P_emp"
        'End If
        'If mon >= 3 Then
        '    str = str & "    , isnull(SUM(isnull(junereg,0)),0)june_hh_P_emp "
        'End If
        'If mon >= 4 Then
        '    str = str & "    , isnull(SUM(isnull(julyreg,0)),0)july_hh_P_emp"
        'End If
        'If mon >= 5 Then
        '    str = str & "     , isnull(SUM(isnull(augreg,0)),0)aug_hh_P_emp "
        'End If
        'If mon >= 6 Then
        '    str = str & "    , isnull(SUM(isnull(sepreg,0)),0)sep_hh_P_emp "
        'End If
        'If mon >= 7 Then
        '    str = str & "    ,  isnull(SUM(isnull(octreg,0)),0)oct_hh_P_emp"
        'End If
        'If mon >= 8 Then
        '    str = str & "    ,isnull(SUM(isnull(novreg,0)),0)nov_hh_P_emp"
        'End If
        'If mon >= 9 Then
        '    str = str & "     , isnull(SUM(isnull(decreg,0)),0)dec_hh_P_emp"
        'End If
        'If mon >= 10 Then
        '    str = str & "     ,  isnull(SUM(isnull(janreg,0)),0)jan_hh_P_emp"
        'End If
        'If mon >= 11 Then
        '    str = str & "    , isnull(SUM(isnull(febreg,0)),0)feb_hh_P_emp "
        'End If
        'If mon >= 12 Then
        '    str = str & "    , isnull(SUM(isnull(marchreg,0)),0)march_hh_P_emp "
        'End If
        'str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_empprov" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        'str = str & "  where " & cond_p & " group by " & val_code_p & ""
        'cmd = New SqlCommand(str, con)
        'da = New SqlDataAdapter(cmd)
        'da.Fill(ds)
        'cmd.Dispose()

        str = ""

        '***********************************Unemployment Allowance/Amount Payable
        str = " select p." & val_code & " state_code,"
        str = str & " isnull(SUM(case when mon='april' and type='unemp' and p.state_code not in ('02','36') then  isnull(duedays,0) end),0) april_unemp,"
        str = str & " isnull(SUM(case when mon='may' and type='unemp'  and p.state_code not in ('02','36') then  isnull(duedays,0) end),0) may_unemp ,"
        str = str & " isnull(SUM(case when mon='june' and type='unemp'  and p.state_code not in ('02','36') then  isnull(duedays,0) end),0) june_unemp , "
        str = str & " isnull(SUM(case when mon='july' and type='unemp'  and p.state_code not in ('02','36') then  isnull(duedays,0) end),0) july_unemp ,"
        str = str & " isnull(SUM(case when mon='August' and type='unemp' and p.state_code not in ('02','36')  then  isnull(duedays,0) end),0) aug_unemp,"
        str = str & " isnull(SUM(case when mon='September' and type='unemp' and p.state_code not in ('02','36')  then  isnull(duedays,0) end),0) sep_unemp ,"
        str = str & " isnull(SUM(case when mon='October' and type='unemp' and p.state_code not in ('02','36')  then  isnull(duedays,0) end),0) oct_unemp ,"
        str = str & " isnull(SUM(case when mon='November' and type='unemp' and p.state_code not in ('02','36')  then  isnull(duedays,0) end),0) nov_unemp ,"
        str = str & " isnull(SUM(case when mon='December' and type='unemp' and p.state_code not in ('02','36')  then  isnull(duedays,0) end),0) dec_unemp,"
        str = str & " isnull(SUM(case when mon='January' and type='unemp' and p.state_code not in ('02','36')  then  isnull(duedays,0) end),0) jan_unemp ,"
        str = str & " isnull(SUM(case when mon='February' and type='unemp' and p.state_code not in ('02','36')  then  isnull(duedays,0) end),0) feb_unemp ,"
        str = str & " isnull(SUM(case when mon='March' and type='unemp' and p.state_code not in ('02','36')  then  isnull(duedays,0) end),0) march_unemp ,"


        str = str & " round(isnull(SUM(case when mon='april' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) april_unemp_amt,"
        str = str & " round(isnull(SUM(case when mon='may' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) may_unemp_amt ,"
        str = str & " round(isnull(SUM(case when mon='june' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) june_unemp_amt , "
        str = str & " round(isnull(SUM(case when mon='july' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) july_unemp_amt ,"
        str = str & " round(isnull(SUM(case when mon='August' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) aug_unemp_amt,"
        str = str & " round(isnull(SUM(case when mon='September' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) sep_unemp_amt ,"
        str = str & " round(isnull(SUM(case when mon='October' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) oct_unemp_amt ,"
        str = str & " round(isnull(SUM(case when mon='November' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) nov_unemp_amt ,"
        str = str & " round(isnull(SUM(case when mon='December' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) dec_unemp_amt,"
        str = str & " round(isnull(SUM(case when mon='January' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) jan_unemp_amt ,"
        str = str & " round(isnull(SUM(case when mon='February' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) feb_unemp_amt ,"
        str = str & " round(isnull(SUM(case when mon='March' and type='unemp' and p.state_code not in ('02','36')  then  isnull(dueamt,0) end),0),2) march_unemp_amt "


        str = str & " from panchayats_rep" & yr & "  p  "
        str = str & " left outer join nrega_unemp_allow" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & " where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""
        '***********************************Delay Compensation/Amount Payable

        str = "select p." & val_code & " state_code,"
        str = str & "  isnull(SUM(case when mon='april' and type='delay' then  isnull(delay,0) end),0) april_delay,"
        str = str & "  isnull(SUM(case when mon='may' and type='delay' then  isnull(delay,0) end),0) may_delay ,"
        str = str & "  isnull(SUM(case when mon='june' and type='delay' then  isnull(delay,0) end),0) june_delay , "
        str = str & "  isnull(SUM(case when mon='july' and type='delay' then  isnull(delay,0) end),0) july_delay ,"
        str = str & "  isnull(SUM(case when mon='August' and type='delay' then  isnull(delay,0) end),0) aug_delay,"
        str = str & "  isnull(SUM(case when mon='September' and type='delay' then  isnull(delay,0) end),0) sep_delay ,"
        str = str & "  isnull(SUM(case when mon='October' and type='delay' then  isnull(delay,0) end),0) oct_delay ,"
        str = str & "  isnull(SUM(case when mon='November' and type='delay' then  isnull(delay,0) end),0) nov_delay ,"
        str = str & "  isnull(SUM(case when mon='December' and type='delay' then  isnull(delay,0) end),0) dec_delay,"
        str = str & "  isnull(SUM(case when mon='January' and type='delay' then  isnull(delay,0) end),0) jan_delay ,"
        str = str & "  isnull(SUM(case when mon='February' and type='delay' then  isnull(delay,0) end),0) feb_delay ,"
        str = str & "  isnull(SUM(case when mon='March' and type='delay' then  isnull(delay,0) end),0) march_delay, "

        str = str & "  round(isnull(SUM(case when mon='april' and type='delay' then  isnull(dueamt,0) end),0),2) april_delay_amt,"
        str = str & "  round(isnull(SUM(case when mon='may' and type='delay' then  isnull(dueamt,0) end),0),2) may_delay_amt ,"
        str = str & "  round(isnull(SUM(case when mon='june' and type='delay' then  isnull(dueamt,0) end),0),2) june_delay_amt , "
        str = str & "  round(isnull(SUM(case when mon='july' and type='delay' then  isnull(dueamt,0) end),0),2) july_delay_amt ,"
        str = str & "  round(isnull(SUM(case when mon='August' and type='delay' then  isnull(dueamt,0) end),0),2) aug_delay_amt,"
        str = str & "  round(isnull(SUM(case when mon='September' and type='delay' then  isnull(dueamt,0) end),0),2) sep_delay_amt ,"
        str = str & "  round(isnull(SUM(case when mon='October' and type='delay' then  isnull(dueamt,0) end),0),2) oct_delay_amt ,"
        str = str & "  round(isnull(SUM(case when mon='November' and type='delay' then  isnull(dueamt,0) end),0),2) nov_delay_amt ,"
        str = str & "  round(isnull(SUM(case when mon='December' and type='delay' then  isnull(dueamt,0) end),0),2) dec_delay_amt,"
        str = str & "  round(isnull(SUM(case when mon='January' and type='delay' then  isnull(dueamt,0) end),0),2) jan_delay_amt ,"
        str = str & "  round(isnull(SUM(case when mon='February' and type='delay' then  isnull(dueamt,0) end),0),2) feb_delay_amt ,"
        str = str & "  round(isnull(SUM(case when mon='March' and type='delay' then  isnull(dueamt,0) end),0),2) march_delay_amt "

        str = str & " from panchayats_rep" & yr & "  p  "
        str = str & " left outer join nrega_unemp_allow" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & " where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()
        str = ""

        '***********************************Unpaid Delay/Amount Payable

        str = "select p." & val_code & " state_code,"
        str = str & "  isnull(SUM(case when mon='april' and type='unpaid_delay' then  isnull(delay,0) end),0) april_unpaid_delay,"
        str = str & "  isnull(SUM(case when mon='may' and type='unpaid_delay' then  isnull(delay,0) end),0) may_unpaid_delay ,"
        str = str & "  isnull(SUM(case when mon='june' and type='unpaid_delay' then  isnull(delay,0) end),0) june_unpaid_delay , "
        str = str & "  isnull(SUM(case when mon='july' and type='unpaid_delay' then  isnull(delay,0) end),0) july_unpaid_delay ,"
        str = str & "  isnull(SUM(case when mon='August' and type='unpaid_delay' then  isnull(delay,0) end),0) aug_unpaid_delay,"
        str = str & "  isnull(SUM(case when mon='September' and type='unpaid_delay' then  isnull(delay,0) end),0) sep_unpaid_delay ,"
        str = str & "  isnull(SUM(case when mon='October' and type='unpaid_delay' then  isnull(delay,0) end),0) oct_unpaid_delay ,"
        str = str & "  isnull(SUM(case when mon='November' and type='unpaid_delay' then  isnull(delay,0) end),0) nov_unpaid_delay ,"
        str = str & "  isnull(SUM(case when mon='December' and type='unpaid_delay' then  isnull(delay,0) end),0) dec_unpaid_delay,"
        str = str & "  isnull(SUM(case when mon='January' and type='unpaid_delay' then  isnull(delay,0) end),0) jan_unpaid_delay ,"
        str = str & "  isnull(SUM(case when mon='February' and type='unpaid_delay' then  isnull(delay,0) end),0) feb_unpaid_delay ,"
        str = str & " isnull(SUM(case when mon='March' and type='unpaid_delay' then  isnull(delay,0) end),0) march_unpaid_delay, "

        str = str & " round(isnull(SUM(case when mon='april' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) april_unpaid_delay_amt,"
        str = str & " round(isnull(SUM(case when mon='may' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) may_unpaid_delay_amt ,"
        str = str & " round(isnull(SUM(case when mon='june' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) june_unpaid_delay_amt , "
        str = str & " round(isnull(SUM(case when mon='july' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) july_unpaid_delay_amt ,"
        str = str & " round(isnull(SUM(case when mon='August' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) aug_unpaid_delay_amt,"
        str = str & " round(isnull(SUM(case when mon='September' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) sep_unpaid_delay_amt ,"
        str = str & " round(isnull(SUM(case when mon='October' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) oct_unpaid_delay_amt ,"
        str = str & " round(isnull(SUM(case when mon='November' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) nov_unpaid_delay_amt ,"
        str = str & " round(isnull(SUM(case when mon='December' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) dec_unpaid_delay_amt,"
        str = str & " round(isnull(SUM(case when mon='January' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) jan_unpaid_delay_amt ,"
        str = str & " round(isnull(SUM(case when mon='February' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) feb_unpaid_delay_amt ,"
        str = str & "  round(isnull(SUM(case when mon='March' and type='unpaid_delay' then  isnull(dueamt,0) end),0),2) march_unpaid_delay_amt "

        str = str & " from panchayats_rep" & yr & "  p  "
        str = str & " left outer join nrega_unemp_allow" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & " where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()
        str = ""

        '***********************************Total Works CategoryWise

        str = "select p." & val_code & " state_code,isnull(SUM(case when wrkcat='AV' then totworks end),0)AV_work, "
        str = str & "  isnull(SUM(case when wrkcat='CA' then totworks end),0)CA_work,  "
        str = str & "  isnull(SUM(case when wrkcat='DP' then totworks end),0)DP_work,  "
        str = str & "  isnull(SUM(case when wrkcat='DW' then totworks end),0)DW_work,  "
        str = str & "  isnull(SUM(case when wrkcat='FP' then totworks end),0)FP_work,  "
        str = str & "  isnull(SUM(case when wrkcat='FR' then totworks end),0)FR_work,  "
        str = str & "  isnull(SUM(case when wrkcat='IC' then totworks end),0)IC_work,  "
        str = str & "  isnull(SUM(case when wrkcat='IF' then totworks end),0)IFf_work,  "
        str = str & "  isnull(SUM(case when wrkcat='LD' then totworks end),0)LD_work,  "
        str = str & "  isnull(SUM(case when wrkcat='OP' then totworks end),0)OP_work,  "
        str = str & "  isnull(SUM(case when wrkcat='RC' then totworks end),0)RC_work,  "
        str = str & "  isnull(SUM(case when wrkcat='RS' then totworks end),0)RS_work,  "
        str = str & "  isnull(SUM(case when wrkcat='SK' then totworks end),0)SK_work,  "
        str = str & "  isnull(SUM(case when wrkcat='WC' then totworks end),0)WC_work,  "
        str = str & "  isnull(SUM(case when wrkcat='WH' then totworks end),0)WH_work,  "
        str = str & "  isnull(SUM(case when wrkcat='PG' then totworks end),0)PG_work "
        str = str & "  from panchayats_rep" & yr & " p  left outer join wrkexpnpanch" & yr & " pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where p." & cond & " and status in ('03','04','05') group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()
        str = ""

        '***********************************Expenditure CategoryWise

        str = "select p." & val_code & " state_code,round(isnull(SUM(case when wrkcat='AV' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)AV_exp, "
        str = str & "  round(isnull(SUM(case when wrkcat='CA' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)CA_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='DP' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)DP_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='DW' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)DW_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='FP' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)FP_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='FR' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)FR_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='IC' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)IC_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='IF' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)IFf_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='LD' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)LD_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='OP' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)OP_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='RC' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)RC_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='RS' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)RS_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='SK' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)SK_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='WC' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)WC_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='WH' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)WH_exp,  "
        str = str & "  round(isnull(SUM(case when wrkcat='PG' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)PG_exp "
        str = str & "  from panchayats_rep" & yr & " p  left outer join wrkexpnpanch" & yr & " pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where p." & cond & " and status in ('03','04','05') group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""

        '**********************************Monthwise Wage Expenditure
        Dim strr, str1, str2, str3, str_1 As String


        str = " round(isnull(sum(case when datename(month,dt_paid) ='april' then lab end),0)/100000,2)  apr_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='may' then lab end),0)/100000,2)  may_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='june' then lab end),0)/100000,2)  jun_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='july' then lab end),0)/100000,2)  jul_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='august' then lab end),0)/100000,2)  aug_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='september' then lab end),0)/100000,2)  sep_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='october' then lab end),0)/100000,2)  oct_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='november' then lab end),0)/100000,2)  nov_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='december' then lab end),0)/100000,2)  dec_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='january' then lab end),0)/100000,2)  jan_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='february' then lab end),0)/100000,2)  feb_lab,"
        str = str & " round(isnull(sum(case when datename(month,dt_paid) ='march' then lab end),0)/100000,2)  mar_lab   "
        str = str & " from("


        str1 = "select s.state_code state_code,"

        str2 = "SELECT p.state_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join panchayats_rep" & yr & " p on p.panchayat_code=a.panchayat_code"
        str2 = str2 & " where p." & cond & ""
        str2 = str2 & " group by p.state_code ,dt_paid"
        str2 = str2 & "  union all"
        str2 = str2 & " SELECT p.state_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join blocks_rep" & yr & " p on p.block_code=left(a.panchayat_code,7) "
        str2 = str2 & "  where right(a.panchayat_code,3) ='000'"
        str2 = str2 & " and p." & cond & ""
        str2 = str2 & " group by p.state_code ,dt_paid"
        str2 = str2 & "   union all"
        str2 = str2 & " SELECT p.state_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join districts_rep" & yr & " p on p.district_code=left(a.panchayat_code,4) "
        str2 = str2 & " where right(a.panchayat_code,6) ='000000'"
        str2 = str2 & " and p." & cond & ""
        str2 = str2 & " group by p.state_code ,dt_paid )#cc inner join states s on #cc.state_code=s.state_code"
        str2 = str2 & " group by s.state_code"




        strr = str1 & str & str2

        cmd = New SqlCommand(strr, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()
        strr = ""

        '**********************************Monthwise Wage Expenditure Previous year

        str_1 = " round(isnull(sum(case when datename(month,dt_paid) ='april' then lab end),0)/100000,2)  apr_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='may' then lab end),0)/100000,2)  may_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='june' then lab end),0)/100000,2)  jun_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='july' then lab end),0)/100000,2)  jul_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='august' then lab end),0)/100000,2)  aug_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='september' then lab end),0)/100000,2)  sep_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='october' then lab end),0)/100000,2)  oct_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='november' then lab end),0)/100000,2)  nov_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='december' then lab end),0)/100000,2)  dec_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='january' then lab end),0)/100000,2)  jan_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='february' then lab end),0)/100000,2)  feb_lab_pre,"
        str_1 = str_1 & " round(isnull(sum(case when datename(month,dt_paid) ='march' then lab end),0)/100000,2)  mar_lab_pre   "
        str_1 = str_1 & " from("

        str1 = "select s.state_code state_code,"

        str3 = "SELECT p.state_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & pre_yr & " a inner join panchayats_rep" & pre_yr & " p on p.panchayat_code=a.panchayat_code"
        str3 = str3 & " where p." & cond & ""
        str3 = str3 & " group by p.state_code ,dt_paid"
        str3 = str3 & "  union all"
        str3 = str3 & " SELECT p.state_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & pre_yr & " a inner join blocks_rep" & pre_yr & " p on p.block_code=left(a.panchayat_code,7) "
        str3 = str3 & "  where right(a.panchayat_code,3) ='000'"
        str3 = str3 & " and p." & cond & ""
        str3 = str3 & " group by p.state_code ,dt_paid"
        str3 = str3 & "   union all"
        str3 = str3 & " SELECT p.state_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & pre_yr & " a inner join districts_rep" & pre_yr & " p on p.district_code=left(a.panchayat_code,4) "
        str3 = str3 & " where right(a.panchayat_code,6) ='000000'"
        str3 = str3 & " and p." & cond & ""
        str3 = str3 & " group by p.state_code ,dt_paid )#cc inner join states s on #cc.state_code=s.state_code"
        str3 = str3 & " group by s.state_code"


        strr = str1 & str_1 & str3

        cmd = New SqlCommand(strr, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""


        '***********************HH provided Employement Previous Year
        str = "select p." & val_code & " state_code,"
        str = str & "     isnull(SUM(isnull(aprilreg,0)),0)april_hh_P_emp_pre, isnull(SUM(isnull(mayreg,0)),0)may_hh_P_emp_pre,"
        str = str & "     isnull(SUM(isnull(junereg,0)),0)june_hh_P_emp_pre,isnull(SUM(isnull(julyreg,0)),0)july_hh_P_emp_pre,"
        str = str & "      isnull(SUM(isnull(augreg,0)),0)aug_hh_P_emp_pre, isnull(SUM(isnull(sepreg,0)),0)sep_hh_P_emp_pre, "
        str = str & "      isnull(SUM(isnull(octreg,0)),0)oct_hh_P_emp_pre,isnull(SUM(isnull(novreg,0)),0)nov_hh_P_emp_pre,"
        str = str & "      isnull(SUM(isnull(decreg,0)),0)dec_hh_P_emp_pre,  isnull(SUM(isnull(janreg,0)),0)jan_hh_P_emp_pre,"
        str = str & "     isnull(SUM(isnull(febreg,0)),0)feb_hh_P_emp_pre,isnull(SUM(isnull(marchreg,0)),0)march_hh_P_emp_pre "
        str = str & "    from panchayats_rep" & pre_yr & " p  left outer join mon_wise_empprov" & pre_yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""


        '***********************Average No. of Persondays per Houshold 
        'str = "select " & val_code_p & " state_code,"
        'str = str & "  (case when sum(isnull(aprilreg,0))>0 then sum(isnull(aprilapp,0))/sum(isnull(aprilreg,0)) else 0 end)apr_PD_per_hh, "
        'str = str & "  (case when sum(isnull(mayreg,0))>0 then sum(isnull(mayapp,0))/sum(isnull(mayreg,0)) else 0 end)may_PD_per_hh ,"
        'str = str & " (case when sum(isnull(junereg,0))>0 then sum(isnull(juneapp,0))/sum(isnull(junereg,0)) else 0 end)jun_PD_per_hh ,"
        'str = str & " (case when sum(isnull(julyreg,0))>0 then sum(isnull(julyapp,0))/sum(isnull(julyreg,0)) else 0 end)jul_PD_per_hh ,"
        'str = str & "  (case when sum(isnull(augreg,0))>0 then sum(isnull(augapp,0))/sum(isnull(augreg,0)) else 0 end)aug_PD_per_hh ,"
        'str = str & "  (case when sum(isnull(sepreg,0))>0 then sum(isnull(sepapp,0))/sum(isnull(sepreg,0)) else 0 end)sep_PD_per_hh ,"
        'str = str & "  (case when sum(isnull(octreg,0))>0 then sum(isnull(octapp,0))/sum(isnull(octreg,0)) else 0 end)oct_PD_per_hh ,"
        'str = str & "  (case when sum(isnull(novreg,0))>0 then sum(isnull(novapp,0))/sum(isnull(novreg,0)) else 0 end)nov_PD_per_hh ,"
        'str = str & "  (case when sum(isnull(decreg,0))>0 then sum(isnull(decapp,0))/sum(isnull(decreg,0)) else 0 end)dec_PD_per_hh ,"
        'str = str & "  (case when sum(isnull(janreg,0))>0 then sum(isnull(janapp,0))/sum(isnull(janreg,0)) else 0 end)jan_PD_per_hh ,"
        'str = str & " (case when sum(isnull(febreg,0))>0 then sum(isnull(febapp,0))/sum(isnull(febreg,0)) else 0 end)feb_PD_per_hh ,"
        'str = str & "  (case when sum(isnull(marchreg,0))>0 then sum(isnull(marchapp,0))/sum(isnull(marchreg,0)) else 0 end)mar_PD_per_hh "
        'str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_empprov" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        'str = str & "  where " & cond_p & " group by " & val_code_p & ""

        str = "select p." & val_code & " state_code,"
        str = str & "  sum(isnull(aprilapp,0))aprilapp_c,sum(isnull(aprilreg,0))aprilreg_c , "
        str = str & "  sum(isnull(mayapp,0))mayapp_c,sum(isnull(mayreg,0))mayreg_c ,"
        str = str & " sum(isnull(juneapp,0))juneapp_c,sum(isnull(junereg,0))junereg_c,"
        str = str & " sum(isnull(julyapp,0))julyapp_c,sum(isnull(julyreg,0))julyreg_c,"
        str = str & " sum(isnull(augapp,0))augapp_c,sum(isnull(augreg,0))augreg_c,"
        str = str & "  sum(isnull(sepapp,0))sepapp_c,sum(isnull(sepreg,0))sepreg_c,"
        str = str & " sum(isnull(octapp,0))octapp_c,sum(isnull(octreg,0))octreg_c  ,"
        str = str & "  sum(isnull(novapp,0))novapp_c,sum(isnull(novreg,0))novreg_c  ,"
        str = str & "  sum(isnull(decapp,0))decapp_c,sum(isnull(decreg,0))decreg_c ,"
        str = str & "  sum(isnull(janapp,0))janapp_c,sum(isnull(janreg,0))janreg_c ,"
        str = str & " sum(isnull(febapp,0))febapp_c,sum(isnull(febreg,0))febreg_c,"
        str = str & "  sum(isnull(marchapp,0))marchapp_c,sum(isnull(marchreg,0))marchreg_c "
        str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_empprov" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""


        '***********************Average No. of Persondays per Houshold Previous year
        'str = "select " & val_code_p & " state_code,"
        'str = str & "  (case when sum(isnull(aprilreg,0))>0 then sum(isnull(aprilapp,0))/sum(isnull(aprilreg,0)) else 0 end)apr_PD_per_hh_preyr, "
        'str = str & "  (case when sum(isnull(mayreg,0))>0 then sum(isnull(mayapp,0))/sum(isnull(mayreg,0)) else 0 end)may_PD_per_hh_preyr,"
        'str = str & " (case when sum(isnull(junereg,0))>0 then sum(isnull(juneapp,0))/sum(isnull(junereg,0)) else 0 end)jun_PD_per_hh_preyr,"
        'str = str & " (case when sum(isnull(julyreg,0))>0 then sum(isnull(julyapp,0))/sum(isnull(julyreg,0)) else 0 end)jul_PD_per_hh_preyr,"
        'str = str & "  (case when sum(isnull(augreg,0))>0 then sum(isnull(augapp,0))/sum(isnull(augreg,0)) else 0 end)aug_PD_per_hh_preyr,"
        'str = str & "  (case when sum(isnull(sepreg,0))>0 then sum(isnull(sepapp,0))/sum(isnull(sepreg,0)) else 0 end)sep_PD_per_hh_preyr,"
        'str = str & "  (case when sum(isnull(octreg,0))>0 then sum(isnull(octapp,0))/sum(isnull(octreg,0)) else 0 end)oct_PD_per_hh_preyr,"
        'str = str & "  (case when sum(isnull(novreg,0))>0 then sum(isnull(novapp,0))/sum(isnull(novreg,0)) else 0 end)nov_PD_per_hh_preyr,"
        'str = str & "  (case when sum(isnull(decreg,0))>0 then sum(isnull(decapp,0))/sum(isnull(decreg,0)) else 0 end)dec_PD_per_hh_preyr,"
        'str = str & "  (case when sum(isnull(janreg,0))>0 then sum(isnull(janapp,0))/sum(isnull(janreg,0)) else 0 end)jan_PD_per_hh_preyr,"
        'str = str & " (case when sum(isnull(febreg,0))>0 then sum(isnull(febapp,0))/sum(isnull(febreg,0)) else 0 end)feb_PD_per_hh_preyr,"
        'str = str & "  (case when sum(isnull(marchreg,0))>0 then sum(isnull(marchapp,0))/sum(isnull(marchreg,0)) else 0 end)mar_PD_per_hh_preyr "
        'str = str & "    from panchayats_rep" & pre_yr & " p  left outer join mon_wise_empprov" & pre_yr & "  pp on p.panchayat_code=pp.panchayat_code"
        'str = str & "  where " & cond_p & " group by " & val_code_p & ""

        str = "select p." & val_code & " state_code,"
        str = str & "  sum(isnull(aprilapp,0))aprilapp_p,sum(isnull(aprilreg,0))aprilreg_p , "
        str = str & "  sum(isnull(mayapp,0))mayapp_p,sum(isnull(mayreg,0))mayreg_p ,"
        str = str & " sum(isnull(juneapp,0))juneapp_p,sum(isnull(junereg,0))junereg_p,"
        str = str & " sum(isnull(julyapp,0))julyapp_p,sum(isnull(julyreg,0))julyreg_p,"
        str = str & " sum(isnull(augapp,0))augapp_p,sum(isnull(augreg,0))augreg_p,"
        str = str & "  sum(isnull(sepapp,0))sepapp_p,sum(isnull(sepreg,0))sepreg_p,"
        str = str & " sum(isnull(octapp,0))octapp_p,sum(isnull(octreg,0))octreg_p  ,"
        str = str & "  sum(isnull(novapp,0))novapp_p,sum(isnull(novreg,0))novreg_p  ,"
        str = str & "  sum(isnull(decapp,0))decapp_p,sum(isnull(decreg,0))decreg_p ,"
        str = str & "  sum(isnull(janapp,0))janapp_p,sum(isnull(janreg,0))janreg_p ,"
        str = str & " sum(isnull(febapp,0))febapp_p,sum(isnull(febreg,0))febreg_p,"
        str = str & "  sum(isnull(marchapp,0))marchapp_p,sum(isnull(marchreg,0))marchreg_p "
        str = str & "    from panchayats_rep" & pre_yr & " p  left outer join mon_wise_empprov" & pre_yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        cmd.CommandTimeout = 0
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        cmd.Dispose()

        str = ""
        con.Close()
        '******************************************//////////////////////////////////////////////////////////////////////*********************************************************************
        ''******************************************//////////////////////////////////////////////////////////////////////*********************************************************************


        'For Each table As DataTable In ds.Tables
        '    For Each dr1 As DataRow In table.Rows
        '        For Each col1 As DataColumn In table.Columns

        '            If col1.ColumnName <> "state_code" Then
        '                Try
        '                    dr1(col1.ColumnName) = Convert.ToDouble(table.Compute("SUM(" & col1.ColumnName & ")", Nothing).ToString)
        '                Catch ex As Exception
        '                    dr1(col1.ColumnName) = "grr"
        '                End Try
        '            Else
        '                dr1("State_code") = "Total"
        '            End If
        '            table.Rows.Add(dr1)
        '        Next

        '    Next

        'Next


        'Dim theTable As New DataTable()
        'Dim totalRow = theTable.NewRow
        'For Each col As DataColumn In theTable.Columns
        '    If col.ColumnName <> "Location" Then
        '        Try
        '            totalRow(col.ColumnName) = Convert.ToDouble(theTable.Compute("SUM(CONVERT([" & col.ColumnName & "], CHAR(64)))", Nothing).ToString)
        '        Catch ex As Exception
        '            totalRow(col.ColumnName) = "grr"
        '        End Try
        '    Else
        '        totalRow("Location") = "Total"
        '    End If
        'Next
        'theTable.Rows.Add(totalRow)

        'Return Table


        'Return ds.Tables(0).Rows.ToString

    End Sub
    Public Function serialize_json() As String
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))()

        Dim data As New Dictionary(Of String, Object)
        For Each table As DataTable In ds.Tables
            '   For Each dr1 As DataRow In table.Rows
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
        If data("aprilreg_c") > 0 Then
            data.Add("apr_PD_per_hh", Round(Convert.ToDecimal(data("aprilapp_c") / data("aprilreg_c")), 2))
        End If
        If data("mayreg_c") > 0 Then
            data.Add("may_PD_per_hh", Round(Convert.ToDecimal(data("mayapp_c") / data("mayreg_c")), 2))
        End If
        If data("junereg_c") > 0 Then
            data.Add("jun_PD_per_hh", Round(Convert.ToDecimal(data("juneapp_c") / data("junereg_c")), 2))
        End If
        If data("julyreg_c") > 0 Then
            data.Add("jul_PD_per_hh", Round(Convert.ToDecimal(data("julyapp_c") / data("julyreg_c")), 2))
        End If
        If data("augreg_c") > 0 Then
            data.Add("aug_PD_per_hh", Round(Convert.ToDecimal(data("augapp_c") / data("augreg_c")), 2))
        End If
        If data("sepreg_c") > 0 Then
            data.Add("sep_PD_per_hh", Round(Convert.ToDecimal(data("sepapp_c") / data("sepreg_c")), 2))
        End If
        If data("octreg_c") > 0 Then
            data.Add("oct_PD_per_hh", Round(Convert.ToDecimal(data("octapp_c") / data("octreg_c")), 2))
        End If
        If data("novreg_c") > 0 Then
            data.Add("nov_PD_per_hh", Round(Convert.ToDecimal(data("novapp_c") / data("novreg_c")), 2))
        End If
        If data("decreg_c") > 0 Then
            data.Add("dec_PD_per_hh", Round(Convert.ToDecimal(data("decapp_c") / data("decreg_c")), 2))
        End If
        If data("janreg_c") > 0 Then
            data.Add("jan_PD_per_hh", Round(Convert.ToDecimal(data("janapp_c") / data("janreg_c")), 2))
        End If
        If data("febreg_c") > 0 Then
            data.Add("feb_PD_per_hh", Round(Convert.ToDecimal(data("febapp_c") / data("febreg_c")), 2))
        End If
        If data("marchreg_c") > 0 Then
            data.Add("mar_PD_per_hh", Round(Convert.ToDecimal(data("marchapp_c") / data("marchreg_c")), 2))
        End If




        If data("aprilreg_p") > 0 Then
            data.Add("apr_PD_per_hh_preyr", Round(Convert.ToDecimal(data("aprilapp_p") / data("aprilreg_p")), 2))
        End If
        If data("mayreg_p") > 0 Then
            data.Add("may_PD_per_hh_preyr", Round(Convert.ToDecimal(data("mayapp_p") / data("mayreg_p")), 2))
        End If
        If data("junereg_p") > 0 Then
            data.Add("jun_PD_per_hh_preyr", Round(Convert.ToDecimal(data("juneapp_p") / data("junereg_p")), 2))
        End If
        If data("julyreg_p") > 0 Then
            data.Add("jul_PD_per_hh_preyr", Round(Convert.ToDecimal(data("julyapp_p") / data("julyreg_p")), 2))
        End If
        If data("augreg_p") > 0 Then
            data.Add("aug_PD_per_hh_preyr", Round(Convert.ToDecimal(data("augapp_p") / data("augreg_p")), 2))
        End If
        If data("sepreg_p") > 0 Then
            data.Add("sep_PD_per_hh_preyr", Round(Convert.ToDecimal(data("sepapp_p") / data("sepreg_p")), 2))
        End If
        If data("octreg_p") > 0 Then
            data.Add("oct_PD_per_hh_preyr", Round(Convert.ToDecimal(data("octapp_p") / data("octreg_p")), 2))
        End If
        If data("novreg_p") > 0 Then
            data.Add("nov_PD_per_hh_preyr", Round(Convert.ToDecimal(data("novapp_p") / data("novreg_p")), 2))
        End If
        If data("decreg_p") > 0 Then
            data.Add("dec_PD_per_hh_preyr", Round(Convert.ToDecimal(data("decapp_p") / data("decreg_p")), 2))
        End If
        If data("janreg_p") > 0 Then
            data.Add("jan_PD_per_hh_preyr", Round(Convert.ToDecimal(data("janapp_p") / data("janreg_p")), 2))
        End If
        If data("febreg_p") > 0 Then
            data.Add("feb_PD_per_hh_preyr", Round(Convert.ToDecimal(data("febapp_p") / data("febreg_p")), 2))
        End If
        If data("marchreg_p") > 0 Then
            data.Add("mar_PD_per_hh_preyr", Round(Convert.ToDecimal(data("marchapp_p") / data("marchreg_p")), 2))
        End If

        rows.Add(data)

        Response.Write(serializer.Serialize(rows))
        Return serializer.Serialize(rows)
    End Function
End Class
