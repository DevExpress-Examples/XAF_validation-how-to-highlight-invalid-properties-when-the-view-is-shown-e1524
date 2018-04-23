Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.Data.Filtering

Namespace DXSample.Module
	<DefaultClassOptions> _
	Public Class Master
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _MasterName As String
		<Index(0)> _
		Public Property MasterName() As String
			Get
				Return _MasterName
			End Get
			Set(ByVal value As String)
				SetPropertyValue("MasterName", _MasterName, value)
			End Set
		End Property
		Private _Description As String
		<RuleRequiredField("Master.Description rule required", "Custom;Save"), Index(1)> _
		Public Property Description() As String
			Get
				Return _Description
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Description", _Description, value)
			End Set
		End Property
		<Association("Master-Details")> _
		Public ReadOnly Property Details() As XPCollection(Of Detail)
			Get
				Return GetCollection(Of Detail)("Details")
			End Get
		End Property
	End Class

End Namespace