using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RangePermissionsSimpleExample
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
            #region #RegisterUserList
            richEditControl1.ReplaceService<IUserListService>(new MyUserListService());
            #endregion #RegisterUserList

            #region #RegisterUserGroupList
            richEditControl1.ReplaceService<IUserGroupListService>(new MyGroupListService());
            #endregion #RegisterUserGroupList

            richEditControl1.CreateNewDocument();
            AuthenticateUser();
            CreateRangePermissions();

        }
        private void CreateRangePermissions()
        {
            // Create document ranges.
            DocumentRange rangeAdmin = AppendDocument("Documents\\administrator.docx");
            DocumentRange rangeBody = AppendDocument("Documents\\body.docx");
            DocumentRange rangeSignature = AppendDocument("Documents\\signature.docx");

            // Protect document ranges.
            #region #CreateRangePermissions            
            RangePermissionCollection rangePermissions = richEditControl1.Document.BeginUpdateRangePermissions();

            RangePermission permission = rangePermissions.CreateRangePermission(rangeAdmin);
            permission.UserName = "Nancy Skywalker";
            permission.Group = "Skywalkers";
            rangePermissions.Add(permission);

            RangePermission permission2 = rangePermissions.CreateRangePermission(rangeBody);
            permission2.Group = @"Everyone";
            rangePermissions.Add(permission2);

            RangePermission permission3 = rangePermissions.CreateRangePermission(rangeSignature);
            permission3.Group = "Nihlus";
            rangePermissions.Add(permission3);

            richEditControl1.Document.EndUpdateRangePermissions(rangePermissions);
            // Enforce protection and set password.
            richEditControl1.Document.Protect("123");
            #endregion #CreateRangePermissions
        }
        private DocumentRange AppendDocument(string filename)
        {
            richEditControl1.Document.Paragraphs.Insert(richEditControl1.Document.Range.End);
            DocumentPosition pos = richEditControl1.Document.CreatePosition(richEditControl1.Document.Range.End.ToInt() - 2);
            DocumentRange range = richEditControl1.Document.InsertDocumentContent(pos, filename, DocumentFormat.OpenXml);
            return range;
        }
        private void AuthenticateUser()
        {
            #region #Authentication
            //Define the user credentials:
            richEditControl1.Options.Authentication.UserName = "Nancy Skywalker";
            richEditControl1.Options.Authentication.Group = "Skywalkers";
            #endregion #Authentication

            #region #RangesColor
            //Customize the editable ranges appearance: 
            richEditControl1.Options.RangePermissions.HighlightColor = Color.PapayaWhip;
            richEditControl1.Options.RangePermissions.HighlightBracketsColor = Color.Olive;
            #endregion #RangesColor
        }

    }
    #region #NewUserGroupList
    class MyGroupListService : IUserGroupListService
    {
        List<string> userGroups = CreateUserGroups();

        static List<string> CreateUserGroups()
        {
            List<string> result = new List<string>();
            result.Add(@"Everyone");
            result.Add(@"Administrators");
            result.Add(@"Contributors");
            result.Add(@"Owners");
            result.Add(@"Editors");
            result.Add(@"Current User");
            result.Add("Skywalkers");
            result.Add("Nihlus");
            return result;
        }
        public IList<string> GetUserGroups()
        {
            return userGroups;
        }
    }
    #endregion #NewUserGroupList

    #region #NewUserList
    class MyUserListService : IUserListService
    {
        List<string> users = CreateUsers();

        static List<string> CreateUsers()
        {
            List<string> result = new List<string>();
            result.Add("Nancy Skywalker");
            result.Add("Andrew Nihlus");
            result.Add("Janet Skywalker");
            result.Add("Margaret");
            return result;
        }
        public IList<string> GetUsers()
        {
            return users;
        }
    }
    #endregion #NewUserList
}
