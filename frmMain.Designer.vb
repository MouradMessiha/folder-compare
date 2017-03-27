<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()> _
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
         If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Me.lblOldObject = New System.Windows.Forms.Label
      Me.lblNewObject = New System.Windows.Forms.Label
      Me.txtOldObject = New System.Windows.Forms.RichTextBox
      Me.txtNewObject = New System.Windows.Forms.RichTextBox
      Me.lstFileChanges = New System.Windows.Forms.ListBox
      Me.lblUpdateOnOld = New System.Windows.Forms.Label
      Me.lblUpdateOnNew = New System.Windows.Forms.Label
      Me.btnOpenFolder1 = New System.Windows.Forms.Button
      Me.btnOpenFolder2 = New System.Windows.Forms.Button
      Me.btnCompare = New System.Windows.Forms.Button
      Me.cmdToggleViewer = New System.Windows.Forms.Button
      Me.ctlNewAsciiViewer = New FolderCompare.ctlAsciiViewer
      Me.ctlOldAsciiViewer = New FolderCompare.ctlAsciiViewer
      Me.SuspendLayout()
      '
      'lblOldObject
      '
      Me.lblOldObject.AutoSize = True
      Me.lblOldObject.Location = New System.Drawing.Point(9, 113)
      Me.lblOldObject.Name = "lblOldObject"
      Me.lblOldObject.Size = New System.Drawing.Size(32, 13)
      Me.lblOldObject.TabIndex = 0
      Me.lblOldObject.Text = "File 1"
      '
      'lblNewObject
      '
      Me.lblNewObject.AutoSize = True
      Me.lblNewObject.Location = New System.Drawing.Point(619, 113)
      Me.lblNewObject.Name = "lblNewObject"
      Me.lblNewObject.Size = New System.Drawing.Size(32, 13)
      Me.lblNewObject.TabIndex = 1
      Me.lblNewObject.Text = "File 2"
      '
      'txtOldObject
      '
      Me.txtOldObject.Location = New System.Drawing.Point(12, 133)
      Me.txtOldObject.Name = "txtOldObject"
      Me.txtOldObject.ReadOnly = True
      Me.txtOldObject.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
      Me.txtOldObject.Size = New System.Drawing.Size(604, 657)
      Me.txtOldObject.TabIndex = 6
      Me.txtOldObject.Text = ""
      '
      'txtNewObject
      '
      Me.txtNewObject.Location = New System.Drawing.Point(622, 133)
      Me.txtNewObject.Name = "txtNewObject"
      Me.txtNewObject.ReadOnly = True
      Me.txtNewObject.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
      Me.txtNewObject.Size = New System.Drawing.Size(604, 657)
      Me.txtNewObject.TabIndex = 7
      Me.txtNewObject.Text = ""
      '
      'lstFileChanges
      '
      Me.lstFileChanges.FormattingEnabled = True
      Me.lstFileChanges.Location = New System.Drawing.Point(12, 12)
      Me.lstFileChanges.Name = "lstFileChanges"
      Me.lstFileChanges.Size = New System.Drawing.Size(1022, 95)
      Me.lstFileChanges.TabIndex = 2
      '
      'lblUpdateOnOld
      '
      Me.lblUpdateOnOld.AutoSize = True
      Me.lblUpdateOnOld.Location = New System.Drawing.Point(492, 113)
      Me.lblUpdateOnOld.Name = "lblUpdateOnOld"
      Me.lblUpdateOnOld.Size = New System.Drawing.Size(0, 13)
      Me.lblUpdateOnOld.TabIndex = 3
      '
      'lblUpdateOnNew
      '
      Me.lblUpdateOnNew.AutoSize = True
      Me.lblUpdateOnNew.Location = New System.Drawing.Point(1101, 113)
      Me.lblUpdateOnNew.Name = "lblUpdateOnNew"
      Me.lblUpdateOnNew.Size = New System.Drawing.Size(0, 13)
      Me.lblUpdateOnNew.TabIndex = 4
      '
      'btnOpenFolder1
      '
      Me.btnOpenFolder1.Location = New System.Drawing.Point(205, 108)
      Me.btnOpenFolder1.Name = "btnOpenFolder1"
      Me.btnOpenFolder1.Size = New System.Drawing.Size(219, 22)
      Me.btnOpenFolder1.TabIndex = 5
      Me.btnOpenFolder1.Text = "Open Folder 1"
      Me.btnOpenFolder1.UseVisualStyleBackColor = True
      '
      'btnOpenFolder2
      '
      Me.btnOpenFolder2.Location = New System.Drawing.Point(815, 108)
      Me.btnOpenFolder2.Name = "btnOpenFolder2"
      Me.btnOpenFolder2.Size = New System.Drawing.Size(219, 22)
      Me.btnOpenFolder2.TabIndex = 6
      Me.btnOpenFolder2.Text = "Open Folder 2"
      Me.btnOpenFolder2.UseVisualStyleBackColor = True
      '
      'btnCompare
      '
      Me.btnCompare.Location = New System.Drawing.Point(1049, 12)
      Me.btnCompare.Name = "btnCompare"
      Me.btnCompare.Size = New System.Drawing.Size(168, 36)
      Me.btnCompare.TabIndex = 7
      Me.btnCompare.Text = "Compare"
      Me.btnCompare.UseVisualStyleBackColor = True
      '
      'cmdToggleViewer
      '
      Me.cmdToggleViewer.Location = New System.Drawing.Point(1042, 92)
      Me.cmdToggleViewer.Name = "cmdToggleViewer"
      Me.cmdToggleViewer.Size = New System.Drawing.Size(39, 15)
      Me.cmdToggleViewer.TabIndex = 10
      Me.cmdToggleViewer.UseVisualStyleBackColor = True
      '
      'ctlNewAsciiViewer
      '
      Me.ctlNewAsciiViewer.Location = New System.Drawing.Point(622, 133)
      Me.ctlNewAsciiViewer.Name = "ctlNewAsciiViewer"
      Me.ctlNewAsciiViewer.Size = New System.Drawing.Size(604, 657)
      Me.ctlNewAsciiViewer.TabIndex = 9
      Me.ctlNewAsciiViewer.TextDisplayed = ""
      Me.ctlNewAsciiViewer.Visible = False
      '
      'ctlOldAsciiViewer
      '
      Me.ctlOldAsciiViewer.Location = New System.Drawing.Point(12, 133)
      Me.ctlOldAsciiViewer.Name = "ctlOldAsciiViewer"
      Me.ctlOldAsciiViewer.Size = New System.Drawing.Size(604, 657)
      Me.ctlOldAsciiViewer.TabIndex = 8
      Me.ctlOldAsciiViewer.TextDisplayed = ""
      Me.ctlOldAsciiViewer.Visible = False
      '
      'frmMain
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(1229, 800)
      Me.Controls.Add(Me.ctlNewAsciiViewer)
      Me.Controls.Add(Me.ctlOldAsciiViewer)
      Me.Controls.Add(Me.cmdToggleViewer)
      Me.Controls.Add(Me.btnCompare)
      Me.Controls.Add(Me.btnOpenFolder2)
      Me.Controls.Add(Me.btnOpenFolder1)
      Me.Controls.Add(Me.lblUpdateOnNew)
      Me.Controls.Add(Me.lblUpdateOnOld)
      Me.Controls.Add(Me.lstFileChanges)
      Me.Controls.Add(Me.txtNewObject)
      Me.Controls.Add(Me.txtOldObject)
      Me.Controls.Add(Me.lblNewObject)
      Me.Controls.Add(Me.lblOldObject)
      Me.Name = "frmMain"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.Text = "Folder Compare"
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents lblOldObject As System.Windows.Forms.Label
   Friend WithEvents lblNewObject As System.Windows.Forms.Label
   Friend WithEvents txtOldObject As System.Windows.Forms.RichTextBox
   Friend WithEvents txtNewObject As System.Windows.Forms.RichTextBox
   Friend WithEvents lstFileChanges As System.Windows.Forms.ListBox
   Friend WithEvents lblUpdateOnOld As System.Windows.Forms.Label
   Friend WithEvents lblUpdateOnNew As System.Windows.Forms.Label
   Friend WithEvents btnOpenFolder1 As System.Windows.Forms.Button
   Friend WithEvents btnOpenFolder2 As System.Windows.Forms.Button
   Friend WithEvents btnCompare As System.Windows.Forms.Button
   Friend WithEvents cmdToggleViewer As System.Windows.Forms.Button
   Friend WithEvents ctlOldAsciiViewer As FolderCompare.ctlAsciiViewer
   Friend WithEvents ctlNewAsciiViewer As FolderCompare.ctlAsciiViewer

End Class
