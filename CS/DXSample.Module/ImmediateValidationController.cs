using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Validation;
using System.Collections;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Editors;

namespace DXSample.Module {
    public class ImmediateValidationTargetObjectsSelector : ValidationTargetObjectSelector {
        protected override bool NeedToValidateObject(DevExpress.Xpo.Session session, object targetObject) {
            return true;
        }
    }
    public class ImmediateValidationController : ViewController {
        private void ValidateObjects(IEnumerable targets) {
            RuleSetValidationResult result = Validator.RuleSet.ValidateAllTargets(targets);

            List<ResultsHighlightController> resultsHighlightControllers = new List<ResultsHighlightController>();
            resultsHighlightControllers.Add(Frame.GetController<ResultsHighlightController>());
            if (View is DetailView) {
                foreach (ListPropertyEditor listPropertyEditor in ((DetailView)View).GetItems<ListPropertyEditor>()) {
                    ResultsHighlightController nestedController = listPropertyEditor.Frame.GetController<ResultsHighlightController>();
                    if (nestedController != null) {
                        resultsHighlightControllers.Add(nestedController);
                    }
                }
            }
            foreach (ResultsHighlightController resultsHighlightController in resultsHighlightControllers) {
                resultsHighlightController.ClearHighlighting();
                if (result.State == ValidationState.Invalid) {
                    resultsHighlightController.HighlightResults(result);
                }
            }
        }
        private void ValidateViewObjects() {
            if (View != null) {
                if (View is ListView) {
                    if (Frame.IsViewControllersActivation) {
                        ValidateObjects(((ListView)View).CollectionSource.List);
                    }
                }
                else {
                    ImmediateValidationTargetObjectsSelector objectsSelector = new ImmediateValidationTargetObjectsSelector();
                    ValidateObjects(objectsSelector.GetObjectsToValidate(((ObjectSpace)View.ObjectSpace).Session, View.CurrentObject));
                }
            }        }
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e) {
            ValidateViewObjects();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            View.ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
            if (View is DetailView) {
                View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
            }
            ValidateViewObjects();
        }

        void View_CurrentObjectChanged(object sender, EventArgs e) {
            ValidateViewObjects();
        }
        protected override void OnDeactivated() {
            View.ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
            if (View is DetailView) {
                View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            }
            base.OnDeactivated();
        }
        public ImmediateValidationController() {
            TargetViewType = DevExpress.ExpressApp.ViewType.Any;
            TargetViewNesting = Nesting.Any;
        }
    }
}
