Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation

Namespace DXSample.Module
	<DefaultClassOptions> _
	Public Class Detail
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _DetailName As String
		<RuleRequiredField("Detail.DetailName rule required", "Custom;Save")> _
		Public Property DetailName() As String
			Get
				Return _DetailName
			End Get
			Set(ByVal value As String)
				SetPropertyValue("DetailName", _DetailName, value)
			End Set
		End Property
		Private _Master As Master
		<Association("Master-Details")> _
		Public Property Master() As Master
			Get
				Return _Master
			End Get
			Set(ByVal value As Master)
				SetPropertyValue("Master", _Master, value)
			End Set
		End Property
	End Class
End Namespace
