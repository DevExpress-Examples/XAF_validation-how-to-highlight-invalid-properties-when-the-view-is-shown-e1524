Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Validation
Imports System.Collections
Imports DevExpress.Persistent.Validation
Imports DevExpress.ExpressApp.Editors

Namespace DXSample.Module
	Public Class ImmediateValidationTargetObjectsSelector
		Inherits ValidationTargetObjectSelector
		Protected Overrides Function NeedToValidateObject(ByVal session As DevExpress.Xpo.Session, ByVal targetObject As Object) As Boolean
			Return True
		End Function
	End Class
	Public Class ImmediateValidationController
		Inherits ViewController
		Private Sub ValidateObjects(ByVal targets As IEnumerable)
			Dim result As RuleSetValidationResult = Validator.RuleSet.ValidateAllTargets(targets)

			Dim resultsHighlightControllers As New List(Of ResultsHighlightController)()
			resultsHighlightControllers.Add(Frame.GetController(Of ResultsHighlightController)())
			If TypeOf View Is DetailView Then
				For Each listPropertyEditor As ListPropertyEditor In (CType(View, DetailView)).GetItems(Of ListPropertyEditor)()
					Dim nestedController As ResultsHighlightController = listPropertyEditor.Frame.GetController(Of ResultsHighlightController)()
					If nestedController IsNot Nothing Then
						resultsHighlightControllers.Add(nestedController)
					End If
				Next listPropertyEditor
			End If
			For Each resultsHighlightController As ResultsHighlightController In resultsHighlightControllers
				resultsHighlightController.ClearHighlighting()
				If result.State = ValidationState.Invalid Then
					resultsHighlightController.HighlightResults(result)
				End If
			Next resultsHighlightController
		End Sub
		Private Sub ValidateViewObjects()
                                	If View IsNot Nothing Then
				If TypeOf View Is ListView Then
					If Not (Frame.IsViewControllersActivation) Then
						ValidateObjects((CType(View, ListView)).CollectionSource.List)
					End If
				Else
					Dim objectsSelector As New ImmediateValidationTargetObjectsSelector()
					ValidateObjects(objectsSelector.GetObjectsToValidate(View.ObjectSpace.Session, View.CurrentObject))
				End If
			End If		
                                End Sub
		Private Sub ObjectSpace_ObjectChanged(ByVal sender As Object, ByVal e As ObjectChangedEventArgs)
			ValidateViewObjects()
		End Sub
		Protected Overrides Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()
			AddHandler View.ObjectSpace.ObjectChanged, AddressOf ObjectSpace_ObjectChanged
			If TypeOf View Is DetailView Then
				AddHandler View.CurrentObjectChanged, AddressOf View_CurrentObjectChanged
			End If
			ValidateViewObjects()
		End Sub

		Private Sub View_CurrentObjectChanged(ByVal sender As Object, ByVal e As EventArgs)
			ValidateViewObjects()
		End Sub
		Protected Overrides Sub OnDeactivating()
			RemoveHandler View.ObjectSpace.ObjectChanged, AddressOf ObjectSpace_ObjectChanged
			If TypeOf View Is DetailView Then
				RemoveHandler View.CurrentObjectChanged, AddressOf View_CurrentObjectChanged
			End If
			MyBase.OnDeactivating()
		End Sub
		Public Sub New()
			TargetViewType = DevExpress.ExpressApp.ViewType.Any
			TargetViewNesting = Nesting.Any
		End Sub
	End Class
End Namespace
