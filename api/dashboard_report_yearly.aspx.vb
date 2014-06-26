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
    Public str, yr, cond, val_code, val_code_p, cond_p, state_code As String
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


        Demand_registered_yearly(state_code, fin_year)

    End Sub
    Public Function Demand_registered_yearly(ByVal state_code As String, ByVal fin_year As String) As String

        yr = Mid(fin_year, 3, 2) & Mid(fin_year, 8, 2)
        Dim dt As New DataTable()
        Dim ds As New DataSet()
        Dim da As SqlDataAdapter
        con = conobj.connectCitizen(state_code)
        con.Open()

        '***********************************Demand_registered

        cmd = New SqlCommand("select p." & val_code & " code,isnull(SUM(isnull(reghh,0)),0)demand_register  from panchayats_rep" & yr & " p  left outer join demregister_panch" & yr & " pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & "", con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt0")
        cmd.Dispose()


        '***********************************Labour_Budget
        If Request.QueryString("type") = "b" Then
            cmd = New SqlCommand("select " & val_code & " code,isnull(SUM(isnull(manday_prj_curryear,0)),0)labour_budget from labour_budget_GP_trans" & yr & " where " & cond & " and month_code='03' group by " & val_code & "", con)
        Else
            cmd = New SqlCommand("select " & val_code & " code,isnull(SUM(isnull(manday_prj_curryear,0)),0)labour_budget from approved_labour_budget_GP_trans" & yr & " where " & cond & "  and month_code='03' group by " & val_code & "", con)
        End If

        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt1")
        cmd.Dispose()

        '***********************************Work_Allotted

        cmd = New SqlCommand("select p." & val_code & " code,isnull(SUM(isnull(eproh,0)),0)work_alloted  from panchayats_rep" & yr & " p  left outer join demregister_panch" & yr & " pp on p.panchayat_code=pp.panchayat_code where p." & cond & " group by p." & val_code & "", con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt2")
        cmd.Dispose()

        '***********************************Unemployement_Allowance Duedays/DueAmount

        str = ""
        str = "select p." & val_code & " code, "
        str = str & " isnull(sum(isnull(dueamt,0)),0) payable_amount,isnull(sum(isnull(duedays,0)),0) payable_days  from panchayats_rep" & yr & " p  left outer join nrega_unemp_allow" & yr & " pp on p.panchayat_code=pp.panchayat_code"
        str = str & "   where  p." & cond & " and type='unemp'"
        str = str & " Group by p." & val_code & ""
        cmd = New SqlCommand(str, con)
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
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt4")
        cmd.Dispose()

        con.Close()

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
