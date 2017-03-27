<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlAsciiViewer
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
      Me.mnuContext = New System.Windows.Forms.ContextMenuStrip(Me.components)
      Me.mnuShowGrid = New System.Windows.Forms.ToolStripMenuItem
      Me.mnuGoToCharPosition = New System.Windows.Forms.ToolStripMenuItem
      Me.btnGoToPosition = New System.Windows.Forms.Button
      Me.lblGoToPosition = New System.Windows.Forms.Label
      Me.txtGoToPosition = New System.Windows.Forms.TextBox
      Me.mnuContext.SuspendLayout()
      Me.SuspendLayout()
      '
      'mnuContext
      '
      Me.mnuContext.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowGrid, Me.mnuGoToCharPosition})
      Me.mnuContext.Name = "mnuContext"
      Me.mnuContext.Size = New System.Drawing.Size(176, 48)
      '
      'mnuShowGrid
      '
      Me.mnuShowGrid.CheckOnClick = True
      Me.mnuShowGrid.Name = "mnuShowGrid"
      Me.mnuShowGrid.Size = New System.Drawing.Size(175, 22)
      Me.mnuShowGrid.Text = "&Show grid"
      '
      'mnuGoToCharPosition
      '
      Me.mnuGoToCharPosition.Name = "mnuGoToCharPosition"
      Me.mnuGoToCharPosition.Size = New System.Drawing.Size(175, 22)
      Me.mnuGoToCharPosition.Text = "&Go to char position"
      '
      'btnGoToPosition
      '
      Me.btnGoToPosition.Location = New System.Drawing.Point(675, 8)
      Me.btnGoToPosition.Name = "btnGoToPosition"
      Me.btnGoToPosition.Size = New System.Drawing.Size(44, 22)
      Me.btnGoToPosition.TabIndex = 2
      Me.btnGoToPosition.Text = "Go"
      Me.btnGoToPosition.UseVisualStyleBackColor = True
      Me.btnGoToPosition.Visible = False
      '
      'lblGoToPosition
      '
      Me.lblGoToPosition.AutoSize = True
      Me.lblGoToPosition.Location = New System.Drawing.Point(372, 13)
      Me.lblGoToPosition.Name = "lblGoToPosition"
      Me.lblGoToPosition.Size = New System.Drawing.Size(108, 13)
      Me.lblGoToPosition.TabIndex = 0
      Me.lblGoToPosition.Text = "Go To Char Position :"
      Me.lblGoToPosition.Visible = False
      '
      'txtGoToPosition
      '
      Me.txtGoToPosition.Location = New System.Drawing.Point(483, 9)
      Me.txtGoToPosition.Name = "txtGoToPosition"
      Me.txtGoToPosition.Size = New System.Drawing.Size(183, 20)
      Me.txtGoToPosition.TabIndex = 1
      Me.txtGoToPosition.Visible = False
      '
      'ctlAsciiViewer
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ContextMenuStrip = Me.mnuContext
      Me.Controls.Add(Me.btnGoToPosition)
      Me.Controls.Add(Me.lblGoToPosition)
      Me.Controls.Add(Me.txtGoToPosition)
      Me.Name = "ctlAsciiViewer"
      Me.Size = New System.Drawing.Size(1196, 609)
      Me.mnuContext.ResumeLayout(False)
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents mnuContext As System.Windows.Forms.ContextMenuStrip
   Friend WithEvents mnuShowGrid As System.Windows.Forms.ToolStripMenuItem
   Friend WithEvents mnuGoToCharPosition As System.Windows.Forms.ToolStripMenuItem
   Private WithEvents btnGoToPosition As System.Windows.Forms.Button
   Private WithEvents lblGoToPosition As System.Windows.Forms.Label
   Private WithEvents txtGoToPosition As System.Windows.Forms.TextBox

End Class
