<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fristrun
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fristrun))
        Me.close_window = New System.Windows.Forms.PictureBox()
        Me.notice = New System.Windows.Forms.Label()
        Me.shop_list = New System.Windows.Forms.ComboBox()
        Me.In_back0 = New System.Windows.Forms.Label()
        Me.In_back1 = New System.Windows.Forms.Label()
        Me.shop_id_in = New System.Windows.Forms.TextBox()
        Me.Yes_pic = New System.Windows.Forms.PictureBox()
        Me.Yes_Button = New System.Windows.Forms.Label()
        CType(Me.close_window, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Yes_pic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'close_window
        '
        Me.close_window.Image = CType(resources.GetObject("close_window.Image"), System.Drawing.Image)
        Me.close_window.Location = New System.Drawing.Point(597, 12)
        Me.close_window.Name = "close_window"
        Me.close_window.Size = New System.Drawing.Size(40, 40)
        Me.close_window.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.close_window.TabIndex = 0
        Me.close_window.TabStop = False
        '
        'notice
        '
        Me.notice.Font = New System.Drawing.Font("幼圆", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.notice.Location = New System.Drawing.Point(7, 100)
        Me.notice.Name = "notice"
        Me.notice.Size = New System.Drawing.Size(650, 36)
        Me.notice.TabIndex = 1
        Me.notice.Text = "您好，这是您第一次登录，请在下方选择超市或者填写超市编号："
        Me.notice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'shop_list
        '
        Me.shop_list.BackColor = System.Drawing.Color.White
        Me.shop_list.Cursor = System.Windows.Forms.Cursors.Default
        Me.shop_list.Font = New System.Drawing.Font("幼圆", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.shop_list.FormattingEnabled = True
        Me.shop_list.Location = New System.Drawing.Point(125, 188)
        Me.shop_list.Name = "shop_list"
        Me.shop_list.Size = New System.Drawing.Size(400, 29)
        Me.shop_list.TabIndex = 2
        '
        'In_back0
        '
        Me.In_back0.BackColor = System.Drawing.Color.DimGray
        Me.In_back0.Location = New System.Drawing.Point(125, 268)
        Me.In_back0.Name = "In_back0"
        Me.In_back0.Size = New System.Drawing.Size(400, 30)
        Me.In_back0.TabIndex = 3
        '
        'In_back1
        '
        Me.In_back1.BackColor = System.Drawing.Color.White
        Me.In_back1.Location = New System.Drawing.Point(126, 269)
        Me.In_back1.Name = "In_back1"
        Me.In_back1.Size = New System.Drawing.Size(398, 28)
        Me.In_back1.TabIndex = 4
        '
        'shop_id_in
        '
        Me.shop_id_in.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.shop_id_in.Font = New System.Drawing.Font("幼圆", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.shop_id_in.Location = New System.Drawing.Point(126, 271)
        Me.shop_id_in.Name = "shop_id_in"
        Me.shop_id_in.Size = New System.Drawing.Size(398, 24)
        Me.shop_id_in.TabIndex = 5
        '
        'Yes_pic
        '
        Me.Yes_pic.Image = CType(resources.GetObject("Yes_pic.Image"), System.Drawing.Image)
        Me.Yes_pic.Location = New System.Drawing.Point(432, 388)
        Me.Yes_pic.Name = "Yes_pic"
        Me.Yes_pic.Size = New System.Drawing.Size(92, 33)
        Me.Yes_pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.Yes_pic.TabIndex = 6
        Me.Yes_pic.TabStop = False
        '
        'Yes_Button
        '
        Me.Yes_Button.BackColor = System.Drawing.Color.Transparent
        Me.Yes_Button.Font = New System.Drawing.Font("幼圆", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Yes_Button.ForeColor = System.Drawing.Color.White
        Me.Yes_Button.Location = New System.Drawing.Point(0, 0)
        Me.Yes_Button.Name = "Yes_Button"
        Me.Yes_Button.Size = New System.Drawing.Size(92, 33)
        Me.Yes_Button.TabIndex = 7
        Me.Yes_Button.Text = "确定"
        Me.Yes_Button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fristrun
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(650, 470)
        Me.Controls.Add(Me.Yes_Button)
        Me.Controls.Add(Me.Yes_pic)
        Me.Controls.Add(Me.shop_id_in)
        Me.Controls.Add(Me.In_back1)
        Me.Controls.Add(Me.In_back0)
        Me.Controls.Add(Me.shop_list)
        Me.Controls.Add(Me.notice)
        Me.Controls.Add(Me.close_window)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "fristrun"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "fristrun"
        CType(Me.close_window, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Yes_pic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents close_window As System.Windows.Forms.PictureBox
    Friend WithEvents notice As System.Windows.Forms.Label
    Friend WithEvents shop_list As System.Windows.Forms.ComboBox
    Friend WithEvents In_back0 As System.Windows.Forms.Label
    Friend WithEvents In_back1 As System.Windows.Forms.Label
    Friend WithEvents shop_id_in As System.Windows.Forms.TextBox
    Friend WithEvents Yes_pic As System.Windows.Forms.PictureBox
    Friend WithEvents Yes_Button As System.Windows.Forms.Label
End Class
