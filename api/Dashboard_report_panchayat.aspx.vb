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
Partial Class nrega_reportdashboard_api_Dashboard_report_panchayat
    Inherits System.Web.UI.Page
    Dim con, con1 As New SqlConnection
    Dim cmd As New SqlCommand
    Dim myreader, myreader1 As SqlDataReader
    Public str, yr, Block_code, fin_year As String
    Dim conobj As New ConnectNREGA

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("fin_year") = "" Then
            Response.Write("Enter fin Year")
            Response.End()

        End If
        fin_year = Request("fin_year").ToString()
        Block_code = Request("Block_code").ToString()
        Panchayat(Block_code, fin_year)

    End Sub
    Public Function Panchayat(ByVal Block_code As String, ByVal fin_year As String) As String
        yr = Mid(fin_year, 3, 2) & Mid(fin_year, 8, 2)
        Dim dt As New DataTable()
        Dim ds As New DataSet()
        Dim da As SqlDataAdapter
        Dim state_code As String = Left(Block_code, 2)
        con = conobj.connectCitizen(state_code)
        con.Open()

        '**************************************************Panchayat

        str = "select  Panchayat_name,panchayat_code from panchayats_rep"& yr &" where block_code='"& Block_code &"'"
        cmd = New SqlCommand(str, con)
        da = New SqlDataAdapter(cmd)
        da.Fill(ds, "dt")
        cmd.Dispose()

        con.Close()
        '******************************************//////////////////////////////////////////////////////////////////////*********************************************************************
        ''******************************************//////////////////////////////////////////////////////////////////////*********************************************************************
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))()
        Dim i As Integer = 0
        Dim data As New Dictionary(Of String, Object)
        For Each table As DataTable In ds.Tables
            For Each dr1 As DataRow In table.Rows


                For Each col1 As DataColumn In table.Columns



                    data.Add(col1.ColumnName & i & "", dr1(col1))




                Next
                i = i + 1
            Next
            rows.Add(data)
        Next


        Response.Write(serializer.Serialize(rows))
        Return serializer.Serialize(rows)


    End Function

End Class
