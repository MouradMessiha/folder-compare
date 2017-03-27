Imports System
Imports System.Drawing
Imports System.Text
Imports System.IO
Imports System.Math
Imports System.Threading

Public Class ctlAsciiViewer

   Private marrFileContents() As Byte
   Private mobjFormBitmap As Bitmap
   Private mobjBitmapGraphics As Graphics
   Private mintFormWidth As Integer
   Private mintFormHeight As Integer
   Private mintCharacterAreaX1 As Integer
   Private mintCharacterAreaY1 As Integer
   Private mintCharacterAreaX2 As Integer
   Private mintCharacterAreaY2 As Integer
   Private mintAsciiAreaX1 As Integer
   Private mintAsciiAreaY1 As Integer
   Private mintAsciiAreaX2 As Integer
   Private mintAsciiAreaY2 As Integer
   Private mintNumberColumns As Integer = 25
   Private mintNumberRows As Integer = 15
   Private mintCellWidth As Integer
   Private mintCellHeight As Integer
   Private mintStartCharIndex As Integer
   Private mintSelectedCharIndex As Integer
   Private mintSelectionLength As Integer
   Private mlstHighLights As List(Of IndexRange)

   Private Sub ctlAsciiViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      mintFormWidth = Me.Width
      mintFormHeight = Me.Height
      mobjFormBitmap = New Bitmap(mintFormWidth, mintFormHeight, Me.CreateGraphics())
      mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap)
      mintCharacterAreaX1 = 10
      mintCharacterAreaY1 = 30
      mintCharacterAreaX2 = mintFormWidth - 10
      mintCharacterAreaY2 = 30 + (mintFormHeight - 40) / 2
      mintAsciiAreaX1 = mintCharacterAreaX1
      mintAsciiAreaY2 = mintFormHeight - 10
      mintCellWidth = Floor((mintCharacterAreaX2 - mintCharacterAreaX1) / mintNumberColumns)
      mintCellHeight = Floor((mintCharacterAreaY2 - mintCharacterAreaY1) / mintNumberRows)
      mintCharacterAreaX2 = mintCharacterAreaX1 + mintCellWidth * mintNumberColumns    ' recalculate to make it an even number of pixels per column
      mintAsciiAreaX2 = mintCharacterAreaX2
      mintCharacterAreaY2 = mintCharacterAreaY1 + mintCellHeight * mintNumberRows     ' recalculate to make it an even number of pixels per row
      mintAsciiAreaY1 = mintCharacterAreaY2 + 1
      mintAsciiAreaY2 = mintAsciiAreaY1 + mintCellHeight * mintNumberRows             ' recalculate to make it an even number of pixels per row
      marrFileContents = New Byte() {}
      mintStartCharIndex = 0
      mintSelectedCharIndex = 0
      mintSelectionLength = 0
      mlstHighLights = New List(Of IndexRange)
      DisplayFile()

   End Sub

   Private Sub ctlAsciiViewer_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
      e.Graphics.DrawImage(mobjFormBitmap, 0, 0)
   End Sub

   Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)

      ' to remove the flickering

   End Sub

   Protected Overrides Function IsInputKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
      ' don't let the user control lose focus when ther user hits the arrow keys

      If keyData = Keys.Left Or keyData = Keys.Right Or keyData = Keys.Up Or keyData = Keys.Down Then
         Return True
      Else
         Return MyBase.IsInputKey(keyData)
      End If

   End Function

   Private Sub DisplayFile()

      Dim intX As Integer
      Dim intY As Integer
      Dim blnHighLighted As Boolean

      ' white control
      mobjBitmapGraphics.FillRectangle(Brushes.White, 0, 0, mintFormWidth, mintFormHeight)
      ' character area rectangle
      mobjBitmapGraphics.DrawRectangle(Pens.Black, mintCharacterAreaX1, mintCharacterAreaY1, mintCharacterAreaX2 - mintCharacterAreaX1, mintCharacterAreaY2 - mintCharacterAreaY1)
      ' ascii area rectangle
      mobjBitmapGraphics.DrawRectangle(Pens.Black, mintAsciiAreaX1, mintAsciiAreaY1, mintAsciiAreaX2 - mintAsciiAreaX1, mintAsciiAreaY2 - mintAsciiAreaY1)
      ' draw columns lines
      For intCounter As Integer = 1 To mintNumberColumns - 1
         intX = mintCharacterAreaX1 + (intCounter * (mintCharacterAreaX2 - mintCharacterAreaX1) / mintNumberColumns)
         If mnuShowGrid.Checked Then
            mobjBitmapGraphics.DrawLine(Pens.Black, intX, mintCharacterAreaY1, intX, mintAsciiAreaY2)
         End If
      Next

      ' draw row lines in character area
      For intCounter As Integer = 1 To mintNumberRows - 1
         intY = mintCharacterAreaY1 + (intCounter * (mintCharacterAreaY2 - mintCharacterAreaY1) / mintNumberRows)
         If mnuShowGrid.Checked Then
            mobjBitmapGraphics.DrawLine(Pens.Black, mintCharacterAreaX1, intY, mintCharacterAreaX2, intY)
         End If
      Next

      ' draw row lines in ascii area
      For intCounter As Integer = 1 To mintNumberRows - 1
         intY = mintAsciiAreaY1 + (intCounter * (mintAsciiAreaY2 - mintAsciiAreaY1) / mintNumberRows)
         If mnuShowGrid.Checked Then
            mobjBitmapGraphics.DrawLine(Pens.Black, mintAsciiAreaX1, intY, mintAsciiAreaX2, intY)
         End If
      Next

      ' draw line separating the character area from the ascii area
      mobjBitmapGraphics.DrawLine(Pens.Red, mintCharacterAreaX1 + 1, mintCharacterAreaY2, mintCharacterAreaX2, mintCharacterAreaY2)
      mobjBitmapGraphics.DrawLine(Pens.Red, mintAsciiAreaX1 + 1, mintAsciiAreaY1, mintAsciiAreaX2, mintAsciiAreaY1)

      Dim objFont1 As Font
      Dim objFont2 As Font
      objFont1 = New Font("MS Sans Serif", 14, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
      objFont2 = New Font("MS Sans Serif", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)

      ' display the file contents and its ascii codes
      For intYCounter As Integer = 1 To mintNumberRows
         For intXCounter As Integer = 1 To mintNumberColumns

            Dim intCharIndex As Integer      ' index of character to be displayed, zero based
            Dim intCellLeft As Integer
            Dim intCellTop1 As Integer
            Dim intCellTop2 As Integer

            intCharIndex = mintStartCharIndex + intXCounter + ((intYCounter - 1) * mintNumberColumns) - 1

            ' get location where character should be displayed
            intCellLeft = mintCharacterAreaX1 + (((intXCounter - 1) * (mintCharacterAreaX2 - mintCharacterAreaX1)) / mintNumberColumns)
            intCellTop1 = mintCharacterAreaY1 + (((intYCounter - 1) * (mintCharacterAreaY2 - mintCharacterAreaY1)) / mintNumberRows)
            intCellTop2 = mintAsciiAreaY1 + (((intYCounter - 1) * (mintAsciiAreaY2 - mintAsciiAreaY1)) / mintNumberRows)

            If intCharIndex < marrFileContents.Length Then

               mobjBitmapGraphics.FillRectangle(Brushes.LightGray, intCellLeft + 1, intCellTop1 + 1, mintCellWidth - 1, mintCellHeight - 1)
               mobjBitmapGraphics.FillRectangle(Brushes.LightGray, intCellLeft + 1, intCellTop2 + 1, mintCellWidth - 1, mintCellHeight - 1)

               Dim strCharacter As String
               Dim objSize As SizeF

               If CharacterHighlighted(intCharIndex) Then
                  blnHighLighted = True
               Else
                  blnHighLighted = False
               End If

               If blnHighLighted Then
                  ' highlight cell in black
                  mobjBitmapGraphics.FillRectangle(Brushes.Black, intCellLeft + 1, intCellTop1 + 1, mintCellWidth - 1, mintCellHeight - 1)
                  mobjBitmapGraphics.FillRectangle(Brushes.Black, intCellLeft + 1, intCellTop2 + 1, mintCellWidth - 1, mintCellHeight - 1)
               End If

               If intCharIndex >= mintSelectedCharIndex And intCharIndex <= mintSelectedCharIndex + mintSelectionLength - 1 Then
                  ' highlight cell in blue
                  mobjBitmapGraphics.FillRectangle(Brushes.Blue, intCellLeft + 1, intCellTop1 + 1, mintCellWidth - 1, mintCellHeight - 1)
                  mobjBitmapGraphics.FillRectangle(Brushes.Blue, intCellLeft + 1, intCellTop2 + 1, mintCellWidth - 1, mintCellHeight - 1)
                  blnHighLighted = True
               End If

               strCharacter = Encoding.Default.GetString(marrFileContents, intCharIndex, 1)
               objSize = mobjBitmapGraphics.MeasureString(strCharacter, objFont1, 100)

               intX = intCellLeft + (mintCellWidth / 2) - (objSize.Width / 2)
               intY = intCellTop1 + (mintCellHeight / 2) - (objSize.Height / 2)
               If blnHighLighted Then
                  mobjBitmapGraphics.DrawString(strCharacter, objFont1, Brushes.White, intX, intY)
               Else
                  mobjBitmapGraphics.DrawString(strCharacter, objFont1, Brushes.Black, intX, intY)
               End If

               strCharacter = marrFileContents(intCharIndex).ToString()
               objSize = mobjBitmapGraphics.MeasureString(strCharacter, objFont2, 100)

               intX = intCellLeft + (mintCellWidth / 2) - (objSize.Width / 2)
               intY = intCellTop2 + (mintCellHeight / 2) - (objSize.Height / 2)
               If blnHighLighted Then
                  mobjBitmapGraphics.DrawString(strCharacter, objFont2, Brushes.White, intX, intY)
               Else
                  mobjBitmapGraphics.DrawString(strCharacter, objFont2, Brushes.Black, intX, intY)
               End If

               ' highlight the cell if this is the selected character
               If intCharIndex = mintSelectedCharIndex Then
                  mobjBitmapGraphics.DrawRectangle(Pens.Red, intCellLeft + 1, intCellTop1 + 1, mintCellWidth - 1, mintCellHeight - 1)
                  mobjBitmapGraphics.DrawRectangle(Pens.Red, intCellLeft + 2, intCellTop1 + 2, mintCellWidth - 3, mintCellHeight - 3)
                  mobjBitmapGraphics.DrawRectangle(Pens.Red, intCellLeft + 3, intCellTop1 + 3, mintCellWidth - 5, mintCellHeight - 5)

                  mobjBitmapGraphics.DrawRectangle(Pens.Red, intCellLeft + 1, intCellTop2 + 1, mintCellWidth - 1, mintCellHeight - 1)
                  mobjBitmapGraphics.DrawRectangle(Pens.Red, intCellLeft + 2, intCellTop2 + 2, mintCellWidth - 3, mintCellHeight - 3)
                  mobjBitmapGraphics.DrawRectangle(Pens.Red, intCellLeft + 3, intCellTop2 + 3, mintCellWidth - 5, mintCellHeight - 5)

                  ' write the current position on top of the form
                  mobjBitmapGraphics.DrawString("Position: " + mintSelectedCharIndex.ToString() + "       File size: " + marrFileContents.Length.ToString(), objFont1, Brushes.Black, 5, 5)
               End If
            End If
         Next
      Next

      Me.Invalidate()

      ' hide these controls as they are confusing because they have the focus but the form gets the keydown events
      lblGoToPosition.Visible = False
      txtGoToPosition.Visible = False
      btnGoToPosition.Visible = False

   End Sub

   Private Sub mnuShowGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowGrid.Click

      DisplayFile()

   End Sub

   Private Sub ctlAsciiViewer_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick

      Dim intXCellIndex As Integer
      Dim intYCellIndex As Integer
      Dim intNewSelectedIndex As Integer

      If e.X >= mintCharacterAreaX1 And e.X < mintCharacterAreaX2 Then
         If e.Y >= mintCharacterAreaY1 And e.Y < mintCharacterAreaY2 Then       ' mouse clicked in upper rectangle
            intXCellIndex = 1 + Floor((e.X - mintCharacterAreaX1) / ((mintCharacterAreaX2 - mintCharacterAreaX1) / mintNumberColumns))
            intYCellIndex = 1 + Floor((e.Y - mintCharacterAreaY1) / ((mintCharacterAreaY2 - mintCharacterAreaY1) / mintNumberRows))

            intNewSelectedIndex = mintStartCharIndex + intXCellIndex + ((intYCellIndex - 1) * mintNumberColumns) - 1
            If intNewSelectedIndex <= marrFileContents.Length - 1 Then
               mintSelectedCharIndex = intNewSelectedIndex
               mintSelectionLength = 0
               DisplayFile()
            End If
         Else
            If e.Y >= mintAsciiAreaY1 And e.Y < mintAsciiAreaY2 Then                ' mouse clicked in lower rectangle
               intXCellIndex = 1 + Floor((e.X - mintAsciiAreaX1) / ((mintAsciiAreaX2 - mintAsciiAreaX1) / mintNumberColumns))
               intYCellIndex = 1 + Floor((e.Y - mintAsciiAreaY1) / ((mintAsciiAreaY2 - mintAsciiAreaY1) / mintNumberRows))

               intNewSelectedIndex = mintStartCharIndex + intXCellIndex + ((intYCellIndex - 1) * mintNumberColumns) - 1
               If intNewSelectedIndex <= marrFileContents.Length - 1 Then
                  mintSelectedCharIndex = intNewSelectedIndex
                  mintSelectionLength = 0
                  DisplayFile()
               End If
            End If
         End If
      End If

   End Sub

   Private Sub ctlAsciiViewer_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel

      If e.Delta > 0 Then ' scroll up
         If mintStartCharIndex - mintNumberColumns >= 0 Then ' enough to scroll up the view one line
            mintSelectedCharIndex -= mintNumberColumns
            mintStartCharIndex -= mintNumberColumns
            DisplayFile()
         Else
            If (mintSelectedCharIndex - mintNumberColumns >= 0) Then   ' enough space to move up the selected character one line
               mintSelectedCharIndex -= mintNumberColumns
            Else
               mintSelectedCharIndex = 0      ' highlight first character of the file
               DisplayFile()
            End If
         End If
      Else  ' scroll down
         If mintSelectedCharIndex + mintNumberColumns <= marrFileContents.Length - 1 Then   ' enough left in file to move one line down
            mintSelectedCharIndex += mintNumberColumns
            mintStartCharIndex += mintNumberColumns
            DisplayFile()
         Else
            If mintStartCharIndex + mintNumberColumns <= marrFileContents.Length - 1 Then    ' we can still scroll view one line down
               mintStartCharIndex += mintNumberColumns
               mintSelectedCharIndex = marrFileContents.Length - 1      ' highlight last byte in the file
               DisplayFile()
            End If
         End If
      End If

   End Sub

   Private Sub ctlAsciiViewer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

      Dim intNewSelectedIndex As Integer

      Select e.KeyCode
         Case Keys.Right
            If (mintSelectedCharIndex < marrFileContents.Length - 1) Then
               mintSelectedCharIndex += 1
               mintSelectionLength = 0
               If (mintSelectedCharIndex > mintStartCharIndex + (mintNumberRows * mintNumberColumns) - 1) Then
                  mintStartCharIndex += mintNumberColumns
               End If
               DisplayFile()
            End If

         Case Keys.Left
            If (mintSelectedCharIndex > 0) Then
               mintSelectedCharIndex -= 1
               mintSelectionLength = 0
               If (mintSelectedCharIndex < mintStartCharIndex) Then
                  mintStartCharIndex -= mintNumberColumns
               End If
               DisplayFile()
            End If

         Case Keys.Down
            If (mintSelectedCharIndex + mintNumberColumns < marrFileContents.Length) Then  ' can go one line down and not reach eof
               mintSelectedCharIndex += mintNumberColumns
               mintSelectionLength = 0
               If (mintSelectedCharIndex > mintStartCharIndex + (mintNumberRows * mintNumberColumns) - 1) Then  ' need to scroll to show new selection
                  mintStartCharIndex += mintNumberColumns
               End If
               DisplayFile()
            Else   ' go to last character
               mintSelectedCharIndex = marrFileContents.Length - 1     ' highlight last byte in the file
               mintSelectionLength = 0
               If (mintSelectedCharIndex > mintStartCharIndex + (mintNumberRows * mintNumberColumns) - 1) Then ' need to scroll to show new selection
                  mintStartCharIndex += mintNumberColumns
               End If
               DisplayFile()
            End If

         Case Keys.Up
            If (mintSelectedCharIndex - mintNumberColumns >= 0) Then ' can go one line up and not reach bof
               mintSelectedCharIndex -= mintNumberColumns
               mintSelectionLength = 0
               If (mintSelectedCharIndex < mintStartCharIndex) Then ' need to scroll to show new selection
                  mintStartCharIndex -= mintNumberColumns
               End If
               DisplayFile()
            Else  ' we are on the first line, go to first character
               mintSelectedCharIndex = 0
               mintSelectionLength = 0
               DisplayFile()
            End If

         Case Keys.PageDown
            If (mintSelectedCharIndex + (mintNumberRows * mintNumberColumns) <= marrFileContents.Length - 1) Then ' enough left in file to move one page down
               mintSelectedCharIndex += mintNumberRows * mintNumberColumns
               mintSelectionLength = 0
               mintStartCharIndex += mintNumberRows * mintNumberColumns
               DisplayFile()
            Else
               If (mintStartCharIndex + (mintNumberRows * mintNumberColumns) <= marrFileContents.Length - 1) Then ' we can still scroll view one page down
                  mintStartCharIndex += mintNumberRows * mintNumberColumns
               End If
               mintSelectedCharIndex = marrFileContents.Length - 1        ' highlight last byte in the file
               mintSelectionLength = 0
               DisplayFile()
            End If

         Case Keys.PageUp
            If (mintStartCharIndex - (mintNumberRows * mintNumberColumns) >= 0) Then ' enough to page up the view
               mintSelectedCharIndex -= mintNumberRows * mintNumberColumns
               mintSelectionLength = 0
               mintStartCharIndex -= mintNumberRows * mintNumberColumns
               DisplayFile()
            Else
               mintStartCharIndex = 0             ' scroll view up to beginning of file
               If (mintSelectedCharIndex - (mintNumberRows * mintNumberColumns) >= 0) Then  ' enough space to page up the selected character
                  mintSelectedCharIndex -= mintNumberRows * mintNumberColumns
                  mintSelectionLength = 0
               Else
                  mintSelectedCharIndex = 0        ' highlight first character of the file
                  mintSelectionLength = 0
               End If
               DisplayFile()
            End If

         Case Keys.Home
            If (e.Control) Then   ' ctrl-Home
               mintStartCharIndex = 0         ' scroll view up to beginning of file
               mintSelectedCharIndex = 0      ' highlight first character of the file                    
               mintSelectionLength = 0
               DisplayFile()
            Else             ' Home
               mintSelectedCharIndex = Floor(mintSelectedCharIndex / mintNumberColumns)  ' get number of rows before the current selected cell
               mintSelectedCharIndex = mintSelectedCharIndex * mintNumberColumns   ' go to first character on the line
               mintSelectionLength = 0
               DisplayFile()
            End If

         Case Keys.End
            If (e.Control) Then   ' ctrl-End
               mintSelectedCharIndex = marrFileContents.Length - 1   ' highlight last byte in the file
               mintSelectionLength = 0
               mintStartCharIndex = Floor((marrFileContents.Length - 1) / (mintNumberRows * mintNumberColumns))   ' get number of full screen scrolls required to view the end of the file
               mintStartCharIndex = mintStartCharIndex * mintNumberRows * mintNumberColumns    ' scroll down by that number of pages
               DisplayFile()
            Else            ' End
               intNewSelectedIndex = 1 + Floor((mintSelectedCharIndex / mintNumberColumns))    ' get number of rows before and including the current row
               intNewSelectedIndex = (intNewSelectedIndex * mintNumberColumns) - 1     ' go to last character on the line
               If (intNewSelectedIndex <= marrFileContents.Length - 1) Then
                  mintSelectedCharIndex = intNewSelectedIndex
                  mintSelectionLength = 0
               Else
                  mintSelectedCharIndex = marrFileContents.Length - 1
                  mintSelectionLength = 0
               End If
               DisplayFile()
            End If
      End Select

   End Sub

   Private Sub btnGoToPosition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGoToPosition.Click

      GoToPosition(txtGoToPosition.Text)

   End Sub

   Private Sub txtGoToPosition_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGoToPosition.KeyDown

      If (e.KeyCode = Keys.Enter) Then
         GoToPosition(txtGoToPosition.Text)
      End If

   End Sub

   Public Sub GoToPosition(ByVal strCharPosition As String)

      Dim intCharPosition As Integer

      If (Integer.TryParse(strCharPosition, intCharPosition)) Then      ' if it's a valid number
         If (intCharPosition <= marrFileContents.Length - 1 And intCharPosition >= 0) Then    ' if it's within the file length
            mintSelectedCharIndex = intCharPosition
            mintSelectionLength = 0
            If ((mintSelectedCharIndex < mintStartCharIndex) Or (mintSelectedCharIndex > mintStartCharIndex + (mintNumberRows * mintNumberColumns) - 1)) Then
               mintStartCharIndex = Floor(intCharPosition / mintNumberColumns) ' get number of rows before the current selected cell
               mintStartCharIndex = mintStartCharIndex * mintNumberColumns   ' scroll to the current line
            End If
            DisplayFile()
         End If
      End If

   End Sub

   Private Sub mnuGoToCharPosition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuGoToCharPosition.Click

      ' show these controls only when they are needed, as it's confusing that the text box has the focus but the keydown goes to the form
      lblGoToPosition.Visible = True
      txtGoToPosition.Visible = True
      btnGoToPosition.Visible = True
      txtGoToPosition.Focus()

   End Sub

   Public Property TextDisplayed() As String
      Set(ByVal value As String)
         If marrFileContents IsNot Nothing Then  ' if the control has initialized
            marrFileContents = Encoding.Default.GetBytes(value)
            mintStartCharIndex = 0
            mintSelectedCharIndex = 0
            mintSelectionLength = 0
            mlstHighLights.Clear()
            DisplayFile()
         End If
      End Set
      Get
         If marrFileContents IsNot Nothing Then
            Return Encoding.Default.GetString(marrFileContents)
         Else
            Return ""
         End If
      End Get
   End Property

   Public ReadOnly Property TextLength() As Integer
      Get
         Return marrFileContents.Length
      End Get
   End Property

   Public WriteOnly Property FileDisplayed() As String
      Set(ByVal value As String)
         Dim objFileStream As FileStream = New FileStream(value, FileMode.Open, FileAccess.Read)
         Try
            Dim intLength As Integer = objFileStream.Length
            ReDim marrFileContents(intLength - 1)
            Dim intCount As Integer
            Dim intOffset As Integer = 0
            Do While (intCount = objFileStream.Read(marrFileContents, intOffset, intLength - intOffset)) > 0
               intOffset += intCount
            Loop
            mintStartCharIndex = 0
            mintSelectedCharIndex = 0
            mintSelectionLength = 0
            mlstHighLights.Clear()
            DisplayFile()
         Finally
            objFileStream.Close()
         End Try
      End Set
   End Property

   Public ReadOnly Property SelectionStart() As Integer
      Get
         Return mintSelectedCharIndex
      End Get
   End Property

   Public ReadOnly Property SelectionLength() As Integer
      Get
         Return mintSelectionLength
      End Get
   End Property


   Public Sub HighLight(ByVal pintStartIndex As Integer, ByVal pintEndIndex As Integer)

      Dim objIndexRange As IndexRange

      objIndexRange = New IndexRange
      objIndexRange.mintStart = pintStartIndex
      objIndexRange.mintEnd = pintEndIndex
      mlstHighLights.Add(objIndexRange)
      DisplayFile()

   End Sub

   Private Function CharacterHighlighted(ByVal pintIndex As Integer) As Boolean

      For Each objIndexRange As IndexRange In mlstHighLights
         If pintIndex >= objIndexRange.mintStart And pintIndex <= objIndexRange.mintEnd Then
            Return True
         End If
      Next

      Return False

   End Function

   Public Function WantEnterKey() As Boolean

      Return txtGoToPosition.Focused

   End Function

   Public Function Find(ByRef pstrSearch As String, ByVal pblnWholeWords As Boolean, Optional ByVal pintStartPosition As Integer = 0) As Integer

      Dim strTextDisplayed As String
      Dim intPosition As Integer

      strTextDisplayed = Encoding.Default.GetString(marrFileContents)
      If pblnWholeWords Then
         intPosition = InstrWholeWord(strTextDisplayed, pstrSearch, pintStartPosition + 1)
      Else
         intPosition = Strings.InStr(pintStartPosition + 1, strTextDisplayed, pstrSearch, CompareMethod.Text)
      End If

      intPosition -= 1
      If intPosition > -1 Then
         If (intPosition <= marrFileContents.Length - 1 And intPosition >= 0) Then    ' if it's within the file length
            mintSelectedCharIndex = intPosition
            mintSelectionLength = Strings.Len(pstrSearch)
            If ((mintSelectedCharIndex < mintStartCharIndex) Or (mintSelectedCharIndex > mintStartCharIndex + (mintNumberRows * mintNumberColumns) - 1)) Then
               mintStartCharIndex = Floor((intPosition) / mintNumberColumns) ' get number of rows before the current selected cell
               mintStartCharIndex = mintStartCharIndex * mintNumberColumns   ' scroll to the current line
            End If
            DisplayFile()
         End If
      End If

      Return intPosition

   End Function

   Private Function InstrWholeWord(ByRef pstrString1 As String, ByRef pstrString2 As String, Optional ByVal pintStartPosition As Integer = 1) As Integer

      Dim intPosition As Integer
      Dim strCharBefore As String
      Dim strCharAfter As String

      intPosition = Strings.InStr(pintStartPosition, pstrString1, pstrString2, CompareMethod.Text)
      Do While intPosition <> 0
         If intPosition = 1 Then
            strCharBefore = " "
         Else
            strCharBefore = Strings.Mid(pstrString1, intPosition - 1, 1)
         End If

         If intPosition + Strings.Len(pstrString2) - 1 = Strings.Len(pstrString1) Then
            strCharAfter = " "
         Else
            strCharAfter = Strings.Mid(pstrString1, intPosition + Strings.Len(pstrString2), 1)
         End If

         If (Strings.Asc(strCharBefore) < Strings.Asc("A") Or Strings.Asc(strCharBefore) > Strings.Asc("Z")) And _
            (Strings.Asc(strCharBefore) < Strings.Asc("a") Or Strings.Asc(strCharBefore) > Strings.Asc("z")) And _
            (Strings.Asc(strCharBefore) < Strings.Asc("0") Or Strings.Asc(strCharBefore) > Strings.Asc("9")) And _
            (Strings.Asc(strCharAfter) < Strings.Asc("A") Or Strings.Asc(strCharAfter) > Strings.Asc("Z")) And _
            (Strings.Asc(strCharAfter) < Strings.Asc("a") Or Strings.Asc(strCharAfter) > Strings.Asc("z")) And _
            (Strings.Asc(strCharAfter) < Strings.Asc("0") Or Strings.Asc(strCharAfter) > Strings.Asc("9")) Then
            Return intPosition
         End If

         intPosition = Strings.InStr(intPosition + 1, pstrString1, pstrString2, CompareMethod.Text)
      Loop

      Return 0

   End Function

   Public Sub FlashControl(ByVal pintFlashCount As Integer)

      For intCount As Integer = 1 To pintFlashCount
         mobjBitmapGraphics.FillRectangle(Brushes.Gray, 0, 0, mintFormWidth, mintFormHeight)
         Me.Invalidate()
         My.Application.DoEvents()
         Thread.Sleep(100)
         DisplayFile()
         My.Application.DoEvents()
         Thread.Sleep(50)
      Next

   End Sub

End Class


Public Class IndexRange

   Public mintStart As Integer
   Public mintEnd As Integer

End Class