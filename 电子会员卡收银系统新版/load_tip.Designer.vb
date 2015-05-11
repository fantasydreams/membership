<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class load_tip
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意:  以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(load_tip))
        Me.gif = New System.Windows.Forms.PictureBox()
        Me.tip = New System.Windows.Forms.Label()
        CType(Me.gif, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gif
        '
        Me.gif.Image = CType(resources.GetObject("gif.Image"), System.Drawing.Image)
        Me.gif.Location = New System.Drawing.Point(30, 20)
        Me.gif.Name = "gif"
        Me.gif.Size = New System.Drawing.Size(60, 60)
        Me.gif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.gif.TabIndex = 0
        Me.gif.TabStop = False
        '
        'tip
        '
        Me.tip.Font = New System.Drawing.Font("宋体", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.tip.Location = New System.Drawing.Point(120, 20)
        Me.tip.Name = "tip"
        Me.tip.Size = New System.Drawing.Size(248, 60)
        Me.tip.TabIndex = 1
        Me.tip.Text = "请耐心等待程序配置完毕..."
        Me.tip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'load_tip
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(400, 100)
        Me.Controls.Add(Me.tip)
        Me.Controls.Add(Me.gif)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "load_tip"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "load_tip"
        CType(Me.gif, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gif As System.Windows.Forms.PictureBox
    Friend WithEvents tip As System.Windows.Forms.Label
End Class
