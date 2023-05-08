using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace JCI.RetailSurveyTool.DataBase.Models
{
    public class Issue : INotifyPropertyChanged
    {
        private IssueType issueType;
        private IssueCategory issueCategory;


        [Key]
        public int ID { set; get; }
        public int AuditID { set; get; }
        [SQLite.Ignore]
        public virtual Audit Audit { set; get; }
        public int StoreAreaID { set; get; }
        [SQLite.Ignore]
        public virtual StoreArea StoreArea { set; get; }
        public int IssueTypeID { set; get; }
        [SQLite.Ignore]
        public virtual IssueType IssueType
        {
            get => issueType; set
            {
                issueType = value;
                NotifyPropertyChanged();
            }
        }
        public int IssueCategoryID { set; get; }
        [SQLite.Ignore]
        public virtual IssueCategory IssueCategory
        {
            get => issueCategory; set
            {
                issueCategory = value;
                NotifyPropertyChanged();
            }
        }
        [MaxLength(100)]
        [Required]
        public string IssueStatusName { set; get; } = "New";


        [SQLite.Ignore]
        public virtual IssueStatus IssueStatus { set; get; }


        [MaxLength(100)]
        [Required]
        public string IssueAreaAssignedName { set; get; } = "Analyst";

        [SQLite.Ignore]
        public virtual IssueArea IssueAreaAssigned { set; get; }

        public string IssueDescription { set; get; }
        public bool Repaired { set; get; }
        [SQLite.Ignore]
        public virtual List<IssueImage> IssueImages { set; get; } = new List<IssueImage>();

        [SQLite.Ignore]
        public virtual List<IssueComment> IssueComments { set; get; } = new List<IssueComment>();

        [SQLite.Ignore]
        [NotMapped]
        public string IssueTypeDescription { set; get; }
        [SQLite.Ignore]
        [NotMapped]
        public string IssueCategoryDescription { set; get; }

        // JCONTIA - New fields for story 13010
        public string NewServiceCall { set; get; }
        public string PONumber { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                if (propertyName == nameof(IssueType))
                {
                    IssueTypeDescription = issueType.Name;
                    NotifyPropertyChanged(nameof(IssueTypeDescription));
                }
                if (propertyName == nameof(IssueCategory))
                {
                    IssueCategoryDescription = issueCategory.Name;
                    NotifyPropertyChanged(nameof(IssueCategoryDescription));
                }
            }
        }



        //private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}


    }
}