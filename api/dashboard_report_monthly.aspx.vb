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


        '***********************************Work_Allotted

        str = "select " & val_code_p & ","
        str = str & "     isnull(SUM(isnull(aprilreg,0)),0)april_work_allot, isnull(SUM(isnull(mayreg,0)),0)may_work_allot,"
        str = str & "     isnull(SUM(isnull(junereg,0)),0)june_work_allot,isnull(SUM(isnull(julyreg,0)),0)july_work_allot,"
        str = str & "      isnull(SUM(isnull(augreg,0)),0)aug_work_allot, isnull(SUM(isnull(sepreg,0)),0)sep_work_allot, "
        str = str & "      isnull(SUM(isnull(octreg,0)),0)oct_work_allot,isnull(SUM(isnull(novreg,0)),0)nov_work_allot,"
        str = str & "      isnull(SUM(isnull(decreg,0)),0)dec_work_allot,  isnull(SUM(isnull(janreg,0)),0)jan_work_allot,"
        str = str & "     isnull(SUM(isnull(febreg,0)),0)feb_work_allot,isnull(SUM(isnull(marchreg,0)),0)march_work_allot "
        str = str & "    from panchayats_rep" & yr & " p  left outer join mon_wise_empprov" & yr & "  pp on p.panchayat_code=pp.panchayat_code"
        str = str & "  where " & cond_p & " group by " & val_code_p & ""
        cmd = New SqlCommand(str, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt2")
        cmd.Dispose()

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
