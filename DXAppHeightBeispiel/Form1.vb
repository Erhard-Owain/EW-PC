Imports System.ComponentModel
Imports System.Text
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon


Public Class Form1
    Sub New()
        InitSkins()
        InitializeComponent()

    End Sub
    Sub InitSkins()
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()

    End Sub

    Shared Sub SearchResult(ByVal Pfade As String, ByVal Args() As String)
        ' Create a PDF document processor.
        Using documentProcessor As New PdfDocumentProcessor()

            ' Define search words.   '
            Dim words() As String = {"ISBN", "ISSN"} 'geändert vom Original

            ' Load a PDF document. 
            documentProcessor.LoadDocument(Pfade)

            ' Specify the search parameters.
            Dim searchParameters As New PdfTextSearchParameters()
            searchParameters.CaseSensitive = True
            searchParameters.WholeWords = True

            For Each word As String In words
                ' Get the search results from the FindText method called with search text and search parameters.
                Dim result As PdfTextSearchResults = documentProcessor.FindText(word, searchParameters)

                ' If the desired text is found, create a rectangle that corresponds to the area containing the found text and fill the rectangle.
                Do While result.Status = PdfTextSearchStatus.Found
                    Using graphics As PdfGraphics = documentProcessor.CreateGraphics()
                        HighlightFoundText(graphics, result, New SolidBrush(Color.FromArgb(130, 55, 155, 255)))
                    End Using
                    result = documentProcessor.FindText(word, searchParameters)
                Loop
            Next word

            ' Save the modified document.
            documentProcessor.SaveDocument("..\..\Temp.pdf")  ' geändert vom Original
        End Using
    End Sub

    Public Shared Sub HighlightFoundText(ByVal graphics As PdfGraphics, ByVal result As PdfTextSearchResults, ByVal brush As SolidBrush)
        For i As Integer = 0 To result.Rectangles.Count - 1
            Dim rect As New RectangleF(New PointF(CSng(result.Rectangles(i).Left), CSng(result.Page.CropBox.Height) - CSng(result.Rectangles(i).Top)), New SizeF(CSng(result.Rectangles(i).Width), CSng(result.Rectangles(i).Height)))

            graphics.FillRectangle(brush, rect)
        Next i
        graphics.AddToPageForeground(result.Page, 72, 72)
    End Sub
End Class
