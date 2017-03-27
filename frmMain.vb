Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Text
Imports System.IO

Public Class frmMain

   Private Sub btnOpenFolder1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenFolder1.Click

      Dim objProcess As Process

      objProcess = New Process
      objProcess.StartInfo.FileName = My.Application.Info.DirectoryPath & "\Folder1"
      objProcess.Start()

   End Sub

   Private Sub btnOpenFolder2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenFolder2.Click

      Dim objProcess As Process

      objProcess = New Process
      objProcess.StartInfo.FileName = My.Application.Info.DirectoryPath & "\Folder2"
      objProcess.Start()

   End Sub

   Private Sub btnCompare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompare.Click

      ListAllFileChanges()

   End Sub


   Private Sub ListAllFileChanges()

      Dim strNewFilePath As String
      Dim strOldFilePath As String

      Cursor.Current = Cursors.AppStarting
      lstFileChanges.Items.Clear()

      ' list files that were added or changed
      For Each strNewFilePath In My.Computer.FileSystem.GetFiles(My.Application.Info.DirectoryPath & "\Folder2", FileIO.SearchOption.SearchAllSubDirectories, "*")
         strOldFilePath = My.Application.Info.DirectoryPath & "\Folder1" & Strings.Mid(strNewFilePath, Strings.Len(My.Application.Info.DirectoryPath & "\Folder2") + 1)
         If My.Computer.FileSystem.FileExists(strOldFilePath) Then
            If My.Computer.FileSystem.GetFileInfo(strNewFilePath).LastWriteTime <> My.Computer.FileSystem.GetFileInfo(strOldFilePath).LastWriteTime Then
               If My.Computer.FileSystem.GetFileInfo(strNewFilePath).Length <> My.Computer.FileSystem.GetFileInfo(strOldFilePath).Length Then
                  lstFileChanges.Items.Add(strNewFilePath)
               Else
                  If FileText(strNewFilePath) <> FileText(strOldFilePath) Then
                     lstFileChanges.Items.Add(strNewFilePath)
                  End If
               End If
            End If
         Else
            lstFileChanges.Items.Add(strNewFilePath)
         End If
      Next

      ' list files that were deleted
      For Each strOldFilePath In My.Computer.FileSystem.GetFiles(My.Application.Info.DirectoryPath & "\Folder1", FileIO.SearchOption.SearchAllSubDirectories, "*")
         strNewFilePath = My.Application.Info.DirectoryPath & "\Folder2" & Strings.Mid(strOldFilePath, Strings.Len(My.Application.Info.DirectoryPath & "\Folder1") + 1)
         If Not My.Computer.FileSystem.FileExists(strNewFilePath) Then
            lstFileChanges.Items.Add(strNewFilePath)
         End If
      Next

      Cursor.Current = Cursors.Default

      If lstFileChanges.Items.Count > 0 Then
         lstFileChanges.SelectedIndex = 0
         lstFileChanges.Focus()
      Else
         If txtOldObject.Visible Then
            txtOldObject.Text = ""
            txtNewObject.Text = ""
         Else
            ctlOldAsciiViewer.TextDisplayed = ""
            ctlNewAsciiViewer.TextDisplayed = ""
         End If

         lblUpdateOnOld.Text = ""
         lblUpdateOnNew.Text = ""
      End If

   End Sub

   Private Sub lstFileChanges_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFileChanges.SelectedIndexChanged

      If DisplayBothFiles(lstFileChanges.SelectedItem.ToString, True) Then
         HighlightDifferences()
      End If

   End Sub

   Private Function DisplayBothFiles(ByVal pstrNewFilePath As String, Optional ByVal pblnAutoSelectViewer As Boolean = False) As Boolean

      Dim blnBothExist As Boolean = True
      Dim strOldFilePath As String
      Dim strTextOld As String = ""
      Dim strTextNew As String = ""

      Cursor.Current = Cursors.AppStarting
      strOldFilePath = My.Application.Info.DirectoryPath & "\Folder1" & Strings.Mid(pstrNewFilePath, Strings.Len(My.Application.Info.DirectoryPath & "\Folder2") + 1)

      If My.Computer.FileSystem.FileExists(strOldFilePath) Then
         strTextOld = FileText(strOldFilePath)
      End If

      If My.Computer.FileSystem.FileExists(pstrNewFilePath) Then
         strTextNew = FileText(pstrNewFilePath)
      End If

      If pblnAutoSelectViewer Then
         If Strings.InStr(strTextOld, Chr(0)) = 0 And Strings.InStr(strTextNew, Chr(0)) = 0 Then
            txtOldObject.Visible = True
            txtNewObject.Visible = True
            ctlOldAsciiViewer.Visible = False
            ctlNewAsciiViewer.Visible = False
         Else
            ctlOldAsciiViewer.Visible = True
            ctlNewAsciiViewer.Visible = True
            txtOldObject.Visible = False
            txtNewObject.Visible = False
         End If
      End If

      If My.Computer.FileSystem.FileExists(strOldFilePath) Then
         If txtOldObject.Visible Then
            txtOldObject.Text = strTextOld
         Else
            ctlOldAsciiViewer.TextDisplayed = strTextOld
         End If
         lblUpdateOnOld.Text = Strings.Format(My.Computer.FileSystem.GetFileInfo(strOldFilePath).LastWriteTime, "yyyy-MM-dd hh:mm:ss tt")
      Else
         If txtOldObject.Visible Then
            txtOldObject.Text = pstrNewFilePath & " was added"
         Else
            ctlOldAsciiViewer.TextDisplayed = pstrNewFilePath & " was added"
         End If
         lblUpdateOnOld.Text = ""
         blnBothExist = False
      End If

      If My.Computer.FileSystem.FileExists(pstrNewFilePath) Then
         If txtOldObject.Visible Then
            txtNewObject.Text = strTextNew
         Else
            ctlNewAsciiViewer.TextDisplayed = strTextNew
         End If
         lblUpdateOnNew.Text = Strings.Format(My.Computer.FileSystem.GetFileInfo(pstrNewFilePath).LastWriteTime, "yyyy-MM-dd hh:mm:ss tt")
      Else
         If txtOldObject.Visible Then
            txtNewObject.Text = pstrNewFilePath & " was deleted"
         Else
            ctlNewAsciiViewer.TextDisplayed = pstrNewFilePath & " was deleted"
         End If
         lblUpdateOnNew.Text = ""
         blnBothExist = False
      End If

      If txtOldObject.Visible Then
         txtOldObject.ScrollToCaret()
         txtNewObject.ScrollToCaret()
      End If

      Cursor.Current = Cursors.Default

      Return blnBothExist

   End Function

   Private Function FileText(ByVal pstrFileName) As String

      Dim arrFileContents() As Byte

      arrFileContents = New Byte() {}
      Dim objFileStream As FileStream = New FileStream(pstrFileName, FileMode.Open, FileAccess.Read)
      Try
         Dim intLength As Integer = objFileStream.Length
         ReDim arrFileContents(intLength - 1)
         Dim intCount As Integer
         Dim intOffset As Integer = 0
         Do While (intCount = objFileStream.Read(arrFileContents, intOffset, intLength - intOffset)) > 0
            intOffset += intCount
         Loop

      Finally
         objFileStream.Close()
      End Try

      Return Encoding.Default.GetString(arrFileContents)

   End Function

   Private Sub HighlightDifferences()

      Dim strOldText As String
      Dim strNewText As String
      Dim intStartComparisonOld As Integer
      Dim intStartOfLineOld As Integer
      Dim intEndOfLineOld As Integer
      Dim intStartComparisonNew As Integer
      Dim intStartOfLineNew As Integer
      Dim strLine As String
      Dim blnFinishedAll As Boolean
      Dim blnFinishedResynchronizing As Boolean
      Dim blnScrolledOnce As Boolean = False
      Dim lngScrollPositionOld As Long = 0
      Dim lngScrollPositionNew As Long = 0
      Dim intSelectionStartOld As Integer
      Dim intSelectionLengthOld As Integer
      Dim intSelectionStartNew As Integer
      Dim intSelectionLengthNew As Integer

      Cursor.Current = Cursors.AppStarting

      If txtOldObject.Visible Then
         strOldText = txtOldObject.Text
         strNewText = txtNewObject.Text
      Else
         strOldText = ctlOldAsciiViewer.TextDisplayed
         strNewText = ctlNewAsciiViewer.TextDisplayed
      End If

      blnFinishedAll = False
      intStartComparisonOld = 1
      intStartComparisonNew = 1

      Do While Not blnFinishedAll
         Do While Mid(strOldText, intStartComparisonOld, 1) = Mid(strNewText, intStartComparisonNew, 1) And blnFinishedAll = False
            If intStartComparisonOld >= Len(strOldText) Or intStartComparisonNew >= Len(strNewText) Then
               blnFinishedAll = True
            Else
               intStartComparisonOld += 1
               intStartComparisonNew += 1
            End If
         Loop

         If Mid(strOldText, intStartComparisonOld, 1) <> Mid(strNewText, intStartComparisonNew, 1) Then
            intStartComparisonOld -= 1
            intStartComparisonNew -= 1
         End If

         If Not blnFinishedAll Then
            blnFinishedResynchronizing = False
            intStartOfLineOld = intStartComparisonOld
         Else
            intSelectionStartOld = intStartComparisonOld
            If Len(strOldText) - intStartComparisonOld + 1 >= 0 Then
               intSelectionLengthOld = Len(strOldText) - intStartComparisonOld + 1
            Else
               intSelectionLengthOld = 0
            End If

            If intSelectionLengthOld > 0 Then
               If txtOldObject.Visible Then
                  txtOldObject.SelectionStart = intSelectionStartOld
                  txtOldObject.SelectionLength = intSelectionLengthOld
                  txtOldObject.SelectionBackColor = Color.Black
                  txtOldObject.SelectionColor = Color.White
               Else
                  ctlOldAsciiViewer.HighLight(intSelectionStartOld, intSelectionStartOld + intSelectionLengthOld - 1)
               End If
            End If

            intSelectionStartNew = intStartComparisonNew
            If Len(strNewText) - intStartComparisonNew + 1 >= 0 Then
               intSelectionLengthNew = Len(strNewText) - intStartComparisonNew + 1
            Else
               intSelectionLengthNew = 0
            End If

            If intSelectionLengthNew > 0 Then
               If txtOldObject.Visible Then
                  txtNewObject.SelectionStart = intSelectionStartNew
                  txtNewObject.SelectionLength = intSelectionLengthNew
                  txtNewObject.SelectionBackColor = Color.Black
                  txtNewObject.SelectionColor = Color.White
               Else
                  ctlNewAsciiViewer.HighLight(intSelectionStartNew, intSelectionStartNew + intSelectionLengthNew - 1)
               End If
            End If

            If Not blnScrolledOnce Then
               If txtOldObject.Visible Then
                  txtOldObject.ScrollToCaret()
                  txtNewObject.ScrollToCaret()
               Else
                  ctlOldAsciiViewer.GoToPosition(intSelectionStartOld)
                  ctlNewAsciiViewer.GoToPosition(intSelectionStartNew)
               End If
               lngScrollPositionOld = intSelectionStartOld
               lngScrollPositionNew = intSelectionStartNew
               blnScrolledOnce = True
            End If
         End If

         Do While Not blnFinishedAll And Not blnFinishedResynchronizing
            If Not blnFinishedAll Then
               intEndOfLineOld = InStr(intStartOfLineOld + 1, strOldText, vbLf)
               If intEndOfLineOld = 0 Then
                  intEndOfLineOld = Len(strOldText)
                  If intStartOfLineOld = intEndOfLineOld Then
                     blnFinishedAll = True
                  End If
               End If

            End If

            If Not blnFinishedAll Then
               strLine = Mid(strOldText, intStartOfLineOld + 1, intEndOfLineOld - intStartOfLineOld)
               If Trim(Replace(strLine, vbLf, "")) = "" Then
                  intStartOfLineNew = 0
               Else
                  If intStartComparisonNew = 0 Then
                     intStartComparisonNew = 1
                  End If
                  intStartOfLineNew = InStr(intStartComparisonNew, strNewText, strLine)
               End If

               If intStartOfLineNew > 0 Then
                  intSelectionStartOld = intStartComparisonOld
                  If intStartOfLineOld - intStartComparisonOld >= 0 Then
                     intSelectionLengthOld = intStartOfLineOld - intStartComparisonOld
                  Else
                     intSelectionLengthOld = 0
                  End If

                  If intSelectionLengthOld > 0 Then
                     If txtOldObject.Visible Then
                        txtOldObject.SelectionStart = intSelectionStartOld
                        txtOldObject.SelectionLength = intSelectionLengthOld
                        txtOldObject.SelectionBackColor = Color.Black
                        txtOldObject.SelectionColor = Color.White
                     Else
                        ctlOldAsciiViewer.HighLight(intSelectionStartOld, intSelectionStartOld + intSelectionLengthOld - 1)
                     End If
                  End If

                  intSelectionStartNew = intStartComparisonNew
                  If intStartOfLineNew - intStartComparisonNew - 1 >= 0 Then
                     intSelectionLengthNew = intStartOfLineNew - intStartComparisonNew - 1
                  Else
                     intSelectionLengthNew = 0
                  End If

                  If intSelectionLengthNew > 0 Then
                     If txtNewObject.Visible Then
                        txtNewObject.SelectionStart = intSelectionStartNew
                        txtNewObject.SelectionLength = intSelectionLengthNew
                        txtNewObject.SelectionBackColor = Color.Black
                        txtNewObject.SelectionColor = Color.White
                     Else
                        ctlNewAsciiViewer.HighLight(intSelectionStartNew, intSelectionStartNew + intSelectionLengthNew - 1)
                     End If
                  End If

                  intStartComparisonOld = intEndOfLineOld
                  intStartComparisonNew = intStartOfLineNew + Len(strLine) - 1
                  blnFinishedResynchronizing = True
                  If Not blnScrolledOnce Then
                     If txtOldObject.Visible Then
                        txtOldObject.ScrollToCaret()
                        txtNewObject.ScrollToCaret()
                     Else
                        ctlOldAsciiViewer.GoToPosition(intSelectionStartOld)
                        ctlNewAsciiViewer.GoToPosition(intSelectionStartNew)
                     End If
                     lngScrollPositionOld = intSelectionStartOld
                     lngScrollPositionNew = intSelectionStartNew
                     blnScrolledOnce = True
                  End If
               Else
                  intStartOfLineOld = intEndOfLineOld
               End If

            Else
               intSelectionStartOld = intStartComparisonOld
               If Len(strOldText) - intStartComparisonOld + 1 >= 0 Then
                  intSelectionLengthOld = Len(strOldText) - intStartComparisonOld + 1
               Else
                  intSelectionLengthOld = 0
               End If
               If intSelectionLengthOld > 0 Then
                  If txtOldObject.Visible Then
                     txtOldObject.SelectionStart = intSelectionStartOld
                     txtOldObject.SelectionLength = intSelectionLengthOld
                     txtOldObject.SelectionBackColor = Color.Black
                     txtOldObject.SelectionColor = Color.White
                  Else
                     ctlOldAsciiViewer.HighLight(intSelectionStartOld, intSelectionStartOld + intSelectionLengthOld - 1)
                  End If
               End If

               intSelectionStartNew = intStartComparisonNew
               If Len(strNewText) - intStartComparisonNew + 1 >= 0 Then
                  intSelectionLengthNew = Len(strNewText) - intStartComparisonNew + 1
               Else
                  intSelectionLengthNew = 0
               End If
               If intSelectionLengthNew > 0 Then
                  If txtOldObject.Visible Then
                     txtNewObject.SelectionStart = intSelectionStartNew
                     txtNewObject.SelectionLength = intSelectionLengthNew
                     txtNewObject.SelectionBackColor = Color.Black
                     txtNewObject.SelectionColor = Color.White
                  Else
                     ctlNewAsciiViewer.HighLight(intSelectionStartNew, intSelectionStartNew + intSelectionLengthNew - 1)
                  End If
               End If

               If Not blnScrolledOnce Then
                  If txtOldObject.Visible Then
                     txtOldObject.ScrollToCaret()
                     txtNewObject.ScrollToCaret()
                  Else
                     ctlOldAsciiViewer.GoToPosition(intSelectionStartOld)
                     ctlNewAsciiViewer.GoToPosition(intSelectionStartNew)
                  End If
                  lngScrollPositionOld = intSelectionStartOld
                  lngScrollPositionNew = intSelectionStartNew
                  blnScrolledOnce = True
               End If
            End If

         Loop

      Loop

      If txtOldObject.Visible Then
         txtOldObject.SelectionStart = lngScrollPositionOld
         txtOldObject.SelectionLength = 0
         txtNewObject.SelectionStart = lngScrollPositionNew
         txtNewObject.SelectionLength = 0
      Else
         ctlOldAsciiViewer.GoToPosition(lngScrollPositionOld)
         ctlNewAsciiViewer.GoToPosition(lngScrollPositionNew)
      End If

      Cursor.Current = Cursors.Default

   End Sub

   Private Sub txtOldObject_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtOldObject.MouseWheel

      txtNewObject.Focus()

   End Sub

   Private Sub txtNewObject_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtNewObject.MouseWheel

      txtOldObject.Focus()

   End Sub

   Private Sub ctlOldAsciiViewer_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ctlOldAsciiViewer.MouseWheel

      ctlNewAsciiViewer.Focus()

   End Sub

   Private Sub ctlNewAsciiViewer_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ctlNewAsciiViewer.MouseWheel

      ctlOldAsciiViewer.Focus()

   End Sub

   Private Sub cmdToggleViewer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdToggleViewer.Click

      If txtOldObject.Visible Then
         ctlOldAsciiViewer.Visible = True
         ctlNewAsciiViewer.Visible = True
         txtOldObject.Visible = False
         txtNewObject.Visible = False
      Else
         txtOldObject.Visible = True
         txtNewObject.Visible = True
         ctlOldAsciiViewer.Visible = False
         ctlNewAsciiViewer.Visible = False
      End If

      If lstFileChanges.Items.Count > 0 Then
         If DisplayBothFiles(lstFileChanges.SelectedItem.ToString) Then
            HighlightDifferences()
         End If
      End If

   End Sub

End Class
