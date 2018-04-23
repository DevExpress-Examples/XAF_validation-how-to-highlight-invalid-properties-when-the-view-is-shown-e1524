using System;

using DevExpress.Xpo;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;

namespace DXSample.Module {
    [DefaultClassOptions]
    public class Master : BaseObject {
        public Master(Session session) : base(session) { }
        private string _MasterName;
        [Index(0)]
        public string MasterName {
            get { return _MasterName; }
            set { SetPropertyValue("MasterName", ref _MasterName, value); }
        }
        private string _Description;
        [RuleRequiredField("Master.Description rule required", "Custom;Save")]
        [Index(1)]
        public string Description {
            get { return _Description; }
            set { SetPropertyValue("Description", ref _Description, value); }
        }
        [Association("Master-Details")]
        public XPCollection<Detail> Details {
            get {
                return GetCollection<Detail>("Details");
            }
        }
    }

}