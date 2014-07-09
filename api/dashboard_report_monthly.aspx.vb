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
Partial Class nrega_reportdashboard_api_dashboard_report_monthly
    Inherits System.Web.UI.Page
    Dim con, con1 As New SqlConnection
    Dim cmd As New SqlCommand
    Dim myreader, myreader1 As SqlDataReader
    Public str, yr, cond, val_code, val_code_p, cond_p, state_code As String
    Dim conobj As New ConnectNREGA

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("fin_year") = "" Then
            Response.Write("Enter fin Year")
            Response.End()

        End If


        Dim fin_year As String = Request("fin_year").ToString()

        If Request.QueryString("type") = "s" Then
            state_code = Request("state_code").ToString()
            val_code = "state_code"
            val_code_p = "Left(p.panchayat_code, 2)"
            cond_p = "LEFT(p.panchayat_code,2)='" & Request.QueryString("state_code") & "'"
            cond = "state_code='" & Request.QueryString("state_code") & "'"
        ElseIf Request.QueryString("type") = "d" Then
            state_code = Left(Request("district_code").ToString(), 2)
            val_code = "district_code"
            val_code_p = "Left(p.panchayat_code, 4)"
            cond_p = "LEFT(p.panchayat_code,4)='" & Request.QueryString("district_code") & "'"
            cond = "district_code='" & Request.QueryString("district_code") & "'"
        ElseIf Request.QueryString("type") = "b" Then
            state_code = Left(Request("block_code").ToString(), 2)
            val_code = "block_code"
            val_code_p = "Left(p.panchayat_code, 7)"
            cond_p = "LEFT(p.panchayat_code,7)='" & Request.QueryString("block_code") & "'"
            cond = "block_code='" & Request.QueryString("block_code") & "'"
        ElseIf Request.QueryString("type") = "p" Then
            state_code = Left(Request("panchayat_code").ToString(), 2)
            val_code = "panchayat_code"
            val_code_p = "p.panchayat_code"
            cond_p = "p.panchayat_code='" & Request.QueryString("panchayat_code") & "'"
            cond = "panchayat_code='" & Request.QueryString("panchayat_code") & "'"
        End If


        Demand_registered_monthly(state_code, fin_year)

    End Sub
    Public Function Demand_registered_monthly(ByVal state_code As String, ByVal fin_year As String) As String
        yr = Mid(fin_year, 3, 2) & Mid(fin_year, 8, 2)
        Dim dt As New DataTable()
        Dim ds As New DataSet()
        Dim da As SqlDataAdapter
        con = conobj.connectCitizen(state_code)
        con.Open()

        '**************************************************Demand_registered

        str = "select  " & val_code_p & ","
        str = str & "     isnull(SUM(isnull(aprilreg,0)),0)april_demand_reg, isnull(SUM(isnull(mayreg,0)),0)may_demand_reg,"
        str = str & "     isnull(SUM(isnull(junereg,0)),0)june_demand_reg,isnull(SUM(isnull(julyreg,0)),0)july_demand_reg,"
        str = str & "      isnull(SUM(isnull(augreg,0)),0)aug_demand_reg, isnull(SUM(isnull(sepreg,0)),0)sep_demand_reg, "
        str = str & "      isnull(SUM(isnull(octreg,0)),0)oct_demand_reg,isnull(SUM(isnull(novreg,0)),0)nov_demand_reg,"
        str = str & "      isnull(SUM(isnull(decreg,0)),0)dec_demand_reg,  isnull(SUM(isnull(janreg,0)),0)jan_demand_reg,"
        str = str & "     isnull(SUM(isnull(febreg,0)),0)feb_demand_reg,isnull(SUM(isnull(marchreg,0)),0)march_demand_reg "
        str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_dmd" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where " & cond_p & " group by " & val_code_p & ""
        cmd = New SqlCommand(str, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt")
        cmd.Dispose()

        str = ""
        '***********************************Labour_Budget

        str = "select " & val_code & ",isnull(SUM(case when month_code='04' then  isnull(manday_prj_curryear,0) end),0) april_lb, "
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
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt1")
        cmd.Dispose()

        str = ""


        '***********************************Work_Allotted

        str = "select " & val_code_p & ","
        str = str & "     isnull(SUM(isnull(aprilAPP,0)),0)april_work_allot, isnull(SUM(isnull(mayAPP,0)),0)may_work_allot,"
        str = str & "     isnull(SUM(isnull(juneAPP,0)),0)june_work_allot,isnull(SUM(isnull(julyAPP,0)),0)july_work_allot,"
        str = str & "      isnull(SUM(isnull(augAPP,0)),0)aug_work_allot, isnull(SUM(isnull(sepAPP,0)),0)sep_work_allot, "
        str = str & "      isnull(SUM(isnull(octAPP,0)),0)oct_work_allot,isnull(SUM(isnull(novAPP,0)),0)nov_work_allot,"
        str = str & "      isnull(SUM(isnull(decAPP,0)),0)dec_work_allot,  isnull(SUM(isnull(janAPP,0)),0)jan_work_allot,"
        str = str & "     isnull(SUM(isnull(febAPP,0)),0)feb_work_allot,isnull(SUM(isnull(marchAPP,0)),0)march_work_allot "
        str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_empprov" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where " & cond_p & " group by " & val_code_p & ""
        cmd = New SqlCommand(str, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt2")
        cmd.Dispose()

        str = ""


        '***********************************HH provided employement

        str = "select " & val_code_p & ","
        str = str & "     isnull(SUM(isnull(aprilreg,0)),0)april_hh_P_emp, isnull(SUM(isnull(mayreg,0)),0)may_hh_P_emp,"
        str = str & "     isnull(SUM(isnull(junereg,0)),0)june_hh_P_emp,isnull(SUM(isnull(julyreg,0)),0)july_hh_P_emp,"
        str = str & "      isnull(SUM(isnull(augreg,0)),0)aug_hh_P_emp, isnull(SUM(isnull(sepreg,0)),0)sep_hh_P_emp, "
        str = str & "      isnull(SUM(isnull(octreg,0)),0)oct_hh_P_emp,isnull(SUM(isnull(novreg,0)),0)nov_hh_P_emp,"
        str = str & "      isnull(SUM(isnull(decreg,0)),0)dec_hh_P_emp,  isnull(SUM(isnull(janreg,0)),0)jan_hh_P_emp,"
        str = str & "     isnull(SUM(isnull(febreg,0)),0)feb_hh_P_emp,isnull(SUM(isnull(marchreg,0)),0)march_hh_P_emp "
        str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_empprov" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where " & cond_p & " group by " & val_code_p & ""
        cmd = New SqlCommand(str, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt3")
        cmd.Dispose()

        str = ""

        '***********************************Unemployment Allowance/Amount Payable
        str = " select p." & val_code & ","
        str = str & " isnull(SUM(case when mon='april' and type='unemp' then  isnull(duedays,0) end),0) april_unemp,"
        str = str & " isnull(SUM(case when mon='may' and type='unemp' then  isnull(duedays,0) end),0) may_unemp ,"
        str = str & " isnull(SUM(case when mon='june' and type='unemp' then  isnull(duedays,0) end),0) june_unemp , "
        str = str & " isnull(SUM(case when mon='july' and type='unemp' then  isnull(duedays,0) end),0) july_unemp ,"
        str = str & " isnull(SUM(case when mon='August' and type='unemp' then  isnull(duedays,0) end),0) aug_unemp,"
        str = str & " isnull(SUM(case when mon='September' and type='unemp' then  isnull(duedays,0) end),0) sep_unemp ,"
        str = str & " isnull(SUM(case when mon='October' and type='unemp' then  isnull(duedays,0) end),0) oct_unemp ,"
        str = str & " isnull(SUM(case when mon='November' and type='unemp' then  isnull(duedays,0) end),0) nov_unemp ,"
        str = str & " isnull(SUM(case when mon='December' and type='unemp' then  isnull(duedays,0) end),0) dec_unemp,"
        str = str & " isnull(SUM(case when mon='January' and type='unemp' then  isnull(duedays,0) end),0) jan_unemp ,"
        str = str & " isnull(SUM(case when mon='February' and type='unemp' then  isnull(duedays,0) end),0) feb_unemp ,"
        str = str & " isnull(SUM(case when mon='March' and type='unemp' then  isnull(duedays,0) end),0) march_unemp ,"


        str = str & " isnull(SUM(case when mon='april' and type='unemp' then  isnull(dueamt,0) end),0) april_unemp_amt,"
        str = str & " isnull(SUM(case when mon='may' and type='unemp' then  isnull(dueamt,0) end),0) may_unemp_amt ,"
        str = str & " isnull(SUM(case when mon='june' and type='unemp' then  isnull(dueamt,0) end),0) june_unemp_amt , "
        str = str & " isnull(SUM(case when mon='july' and type='unemp' then  isnull(dueamt,0) end),0) july_unemp_amt ,"
        str = str & " isnull(SUM(case when mon='August' and type='unemp' then  isnull(dueamt,0) end),0) aug_unemp_amt,"
        str = str & " isnull(SUM(case when mon='September' and type='unemp' then  isnull(dueamt,0) end),0) sep_unemp_amt ,"
        str = str & " isnull(SUM(case when mon='October' and type='unemp' then  isnull(dueamt,0) end),0) oct_unemp_amt ,"
        str = str & " isnull(SUM(case when mon='November' and type='unemp' then  isnull(dueamt,0) end),0) nov_unemp_amt ,"
        str = str & " isnull(SUM(case when mon='December' and type='unemp' then  isnull(dueamt,0) end),0) dec_unemp_amt,"
        str = str & " isnull(SUM(case when mon='January' and type='unemp' then  isnull(dueamt,0) end),0) jan_unemp_amt ,"
        str = str & " isnull(SUM(case when mon='February' and type='unemp' then  isnull(dueamt,0) end),0) feb_unemp_amt ,"
        str = str & " isnull(SUM(case when mon='March' and type='unemp' then  isnull(dueamt,0) end),0) march_unemp_amt "


        str = str & " from panchayats_rep" & yr & "  p  "
        str = str & " left outer join nrega_unemp_allow" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & " where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt4")
        cmd.Dispose()

        str = ""
        '***********************************Delay Compensation/Amount Payable

        str = "select p." & val_code & ","
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

        str = str & "  isnull(SUM(case when mon='april' and type='delay' then  isnull(dueamt,0) end),0) april_delay_amt,"
        str = str & "  isnull(SUM(case when mon='may' and type='delay' then  isnull(dueamt,0) end),0) may_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='june' and type='delay' then  isnull(dueamt,0) end),0) june_delay_amt , "
        str = str & "  isnull(SUM(case when mon='july' and type='delay' then  isnull(dueamt,0) end),0) july_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='August' and type='delay' then  isnull(dueamt,0) end),0) aug_delay_amt,"
        str = str & "  isnull(SUM(case when mon='September' and type='delay' then  isnull(dueamt,0) end),0) sep_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='October' and type='delay' then  isnull(dueamt,0) end),0) oct_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='November' and type='delay' then  isnull(dueamt,0) end),0) nov_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='December' and type='delay' then  isnull(dueamt,0) end),0) dec_delay_amt,"
        str = str & "  isnull(SUM(case when mon='January' and type='delay' then  isnull(dueamt,0) end),0) jan_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='February' and type='delay' then  isnull(dueamt,0) end),0) feb_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='March' and type='delay' then  isnull(dueamt,0) end),0) march_delay_amt "

        str = str & " from panchayats_rep" & yr & "  p  "
        str = str & " left outer join nrega_unemp_allow" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & " where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt5")
        cmd.Dispose()
        str = ""

        '***********************************Unpaid Delay/Amount Payable

        str = "select p." & val_code & ","
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

        str = str & "  isnull(SUM(case when mon='april' and type='unpaid_delay' then  isnull(dueamt,0) end),0) april_unpaid_delay_amt,"
        str = str & "  isnull(SUM(case when mon='may' and type='unpaid_delay' then  isnull(dueamt,0) end),0) may_unpaid_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='june' and type='unpaid_delay' then  isnull(dueamt,0) end),0) june_unpaid_delay_amt , "
        str = str & "  isnull(SUM(case when mon='july' and type='unpaid_delay' then  isnull(dueamt,0) end),0) july_unpaid_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='August' and type='unpaid_delay' then  isnull(dueamt,0) end),0) aug_unpaid_delay_amt,"
        str = str & "  isnull(SUM(case when mon='September' and type='unpaid_delay' then  isnull(dueamt,0) end),0) sep_unpaid_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='October' and type='unpaid_delay' then  isnull(dueamt,0) end),0) oct_unpaid_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='November' and type='unpaid_delay' then  isnull(dueamt,0) end),0) nov_unpaid_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='December' and type='unpaid_delay' then  isnull(dueamt,0) end),0) dec_unpaid_delay_amt,"
        str = str & "  isnull(SUM(case when mon='January' and type='unpaid_delay' then  isnull(dueamt,0) end),0) jan_unpaid_delay_amt ,"
        str = str & "  isnull(SUM(case when mon='February' and type='unpaid_delay' then  isnull(dueamt,0) end),0) feb_unpaid_delay_amt ,"
        str = str & " isnull(SUM(case when mon='March' and type='unpaid_delay' then  isnull(dueamt,0) end),0) march_unpaid_delay_amt "

        str = str & " from panchayats_rep" & yr & "  p  "
        str = str & " left outer join nrega_unemp_allow" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & " where p." & cond & " group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt6")
        cmd.Dispose()
        str = ""

        '***********************************Total Works CategoryWise

        str = "select p." & val_code & ",isnull(SUM(case when wrkcat='AV' then totworks end),0)AV_work, "
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
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt7")
        cmd.Dispose()
        str = ""

        '***********************************Expenditure CategoryWise

        str = "select p." & val_code & ",round(isnull(SUM(case when wrkcat='AV' then ( isnull(act_lab,0)+isnull(act_mat,0) +isnull(act_skilled,0) +isnull(act_tax,0)) end),0),2)AV_exp, "
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
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt8")
        cmd.Dispose()

        str = ""

        '**********************************Monthwise Wage Expenditure
        Dim strr, str1, str2 As String


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
        If Request.QueryString("type") = "s" Then
            str1 = "select s.state_code code,state_name name,"

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
            str2 = str2 & " group by s.state_code,state_name"
        ElseIf Request.QueryString("type") = "d" Then
            str1 = "select s.district_code code,district_name name,"

            str2 = "SELECT p.district_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join panchayats_rep" & yr & " p on p.panchayat_code=a.panchayat_code"
            str2 = str2 & " where p." & cond & ""
            str2 = str2 & " group by p.district_code ,dt_paid"
            str2 = str2 & "  union all"
            str2 = str2 & " SELECT p.district_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join blocks_rep" & yr & " p on p.block_code=left(a.panchayat_code,7) "
            str2 = str2 & " where right(a.panchayat_code,3) ='000'"
            str2 = str2 & " and p." & cond & ""
            str2 = str2 & " group by p.district_code ,dt_paid"
            str2 = str2 & "    union all"
            str2 = str2 & " SELECT p.district_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join districts_rep" & yr & " p on p.district_code=left(a.panchayat_code,4) "
            str2 = str2 & " where right(a.panchayat_code,6) ='000000'"
            str2 = str2 & " and p." & cond & ""
            str2 = str2 & " group by p.district_code ,dt_paid )#cc inner join districts_rep" & yr & " s on #cc.district_code=s.district_code"
            str2 = str2 & " group by  s.district_code,district_name"
        ElseIf Request.QueryString("type") = "b" Then
            str1 = "select s.block_code code,block_name name,"

            str2 = "SELECT p.block_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join panchayats_rep" & yr & " p on p.panchayat_code=a.panchayat_code"
            str2 = str2 & " where p." & cond & ""
            str2 = str2 & " group by p.block_code ,dt_paid"
            str2 = str2 & "    union all"
            str2 = str2 & " SELECT p.block_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join blocks_rep" & yr & " p on p.block_code=left(a.panchayat_code,7) "
            str2 = str2 & " where right(a.panchayat_code,3) ='000'"
            str2 = str2 & "  and p." & cond & ""
            str2 = str2 & " group by p.block_code ,dt_paid"
            str2 = str2 & " )#cc inner join blocks_rep" & yr & " s on #cc.block_code=s.block_code where s." & cond & ""
            str2 = str2 & " group by s.block_code,block_name"
        ElseIf Request.QueryString("type") = "p" Then
            str1 = "select s.panchayat_code code,panchayat_name name,"

            str2 = "SELECT p.panchayat_code,dt_paid,SUM(exp_lab)lab FROM DAYEXPN" & yr & " a inner join panchayats_rep" & yr & " p on p.panchayat_code=a.panchayat_code"
            str2 = str2 & " where p." & cond & ""
            str2 = str2 & " group by p.panchayat_code ,dt_paid"
            str2 = str2 & " )#cc inner join panchayats_rep" & yr & " s on #cc.panchayat_code=s.panchayat_code where s." & cond & ""
            str2 = str2 & " group by s.panchayat_code,panchayat_name"
        End If


        strr = str1 & str & str2

        cmd = New SqlCommand(strr, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt9")
        cmd.Dispose()

        str = ""
        con.Close()
        '******************************************//////////////////////////////////////////////////////////////////////*********************************************************************
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
