Imports DevExpress.XtraRichEdit
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Services
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace RangePermissionsSimpleExample
    Partial Public Class Form1
        Inherits DevExpress.XtraBars.Ribbon.RibbonForm

        Public Sub New()
            InitializeComponent()
            '            #Region "#RegisterUserList"
            richEditControl1.ReplaceService(Of IUserListService)(New MyUserListService())
            '            #End Region ' #RegisterUserList

            '            #Region "#RegisterUserGroupList"
            richEditControl1.ReplaceService(Of IUserGroupListService)(New MyGroupListService())
            '            #End Region ' #RegisterUserGroupList

            richEditControl1.CreateNewDocument()
            AuthenticateUser()
            CreateRangePermissions()

        End Sub
        Private Sub CreateRangePermissions()
            ' Create document ranges.
            Dim rangeAdmin As DocumentRange = AppendDocument("Documents\administrator.docx")
            Dim rangeBody As DocumentRange = AppendDocument("Documents\body.docx")
            Dim rangeSignature As DocumentRange = AppendDocument("Documents\signature.docx")

            ' Protect document ranges.
            '            #Region "#CreateRangePermissions"
            Dim rangePermissions As RangePermissionCollection = richEditControl1.Document.BeginUpdateRangePermissions()

            Dim permission As RangePermission = rangePermissions.CreateRangePermission(rangeAdmin)
            permission.UserName = "Nancy Skywalker"
            permission.Group = "Skywalkers"
            rangePermissions.Add(permission)

            Dim permission2 As RangePermission = rangePermissions.CreateRangePermission(rangeBody)
            permission2.Group = "Everyone"
            rangePermissions.Add(permission2)

            Dim permission3 As RangePermission = rangePermissions.CreateRangePermission(rangeSignature)
            permission3.Group = "Nihlus"
            rangePermissions.Add(permission3)

            richEditControl1.Document.EndUpdateRangePermissions(rangePermissions)
            ' Enforce protection and set password.
            richEditControl1.Document.Protect("123")
            '            #End Region ' #CreateRangePermissions
        End Sub
        Private Function AppendDocument(ByVal filename As String) As DocumentRange
            richEditControl1.Document.Paragraphs.Insert(richEditControl1.Document.Range.End)
            Dim pos As DocumentPosition = richEditControl1.Document.CreatePosition(richEditControl1.Document.Range.End.ToInt() - 2)
            Dim range As DocumentRange = richEditControl1.Document.InsertDocumentContent(pos, filename, DocumentFormat.OpenXml)
            Return range
        End Function
        Private Sub AuthenticateUser()
            '            #Region "#Authentication"
            'Define the user credentials:
            richEditControl1.Options.Authentication.UserName = "Nancy Skywalker"
            richEditControl1.Options.Authentication.Group = "Skywalkers"
            '            #End Region '#Authentication

            '            #Region "#RangesColor"
            'Customize the editable ranges appearance: 
            richEditControl1.Options.RangePermissions.HighlightColor = Color.PapayaWhip
            richEditControl1.Options.RangePermissions.HighlightBracketsColor = Color.Olive
            '            #End Region '#RangesColor
        End Sub

    End Class
#Region "#NewUserGroupList"
    Friend Class MyGroupListService
        Implements IUserGroupListService

        Private userGroups As List(Of String) = CreateUserGroups()

        Private Shared Function CreateUserGroups() As List(Of String)
            Dim result As New List(Of String)()
            result.Add("Everyone")
            result.Add("Administrators")
            result.Add("Contributors")
            result.Add("Owners")
            result.Add("Editors")
            result.Add("Current User")
            result.Add("Skywalkers")
            result.Add("Nihlus")
            Return result
        End Function
        Public Function GetUserGroups() As IList(Of String) Implements IUserGroupListService.GetUserGroups
            Return userGroups
        End Function
    End Class
#End Region ' #NewUserGroupList

#Region "#NewUserList"
    Friend Class MyUserListService
        Implements IUserListService

        Private users As List(Of String) = CreateUsers()

        Private Shared Function CreateUsers() As List(Of String)
            Dim result As New List(Of String)()
            result.Add("Nancy Skywalker")
            result.Add("Andrew Nihlus")
            result.Add("Janet Skywalker")
            result.Add("Margaret")
            Return result
        End Function
        Public Function GetUsers() As IList(Of String) Implements IUserListService.GetUsers
            Return users
        End Function
    End Class
#End Region ' #NewUserList
End Namespace
