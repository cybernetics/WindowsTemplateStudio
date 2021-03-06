﻿'{**
' This code block adds the method `GetContentGridDataAsync()` to the SampleDataService of your project.
'**}
'{[{
Imports System.Threading.Tasks
'}]}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        Private _allOrders As IEnumerable(Of SampleOrder)

        ' TODO WTS: Remove this once your ContentGrid page is displaying real data.
        Public Async Function GetContentGridDataAsync() As Task(Of ObservableCollection(Of SampleOrder))
            If _allOrders Is Nothing Then
                _allOrders = AllOrders()
            End If

            Await Task.CompletedTask

            Return New ObservableCollection(Of SampleOrder)(_allOrders)
        End Function
'}]}
    End Module
End Namespace
