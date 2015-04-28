Imports System.Windows.Forms

Public Class MSG

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub MSGBOX_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '.BackColor = Color.FromArgb(&HFFFFFFFF)  if set backcolor be white,it looks a little strange
        X.Parent = col
        head.Parent = col
        no_button.Parent = no
        yes_button.Parent = yes
    End Sub
    Private Sub frm_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(13) Then   'when press key enter,return ok
            OK_Button_Click(Me, e)
        ElseIf e.KeyChar = ChrW(27) Then  'when press key esc,return cancel
            Cancel_Button_Click(Me, e)
        End If
    End Sub



    Private Sub X_Click(sender As Object, e As EventArgs) Handles X.Click
        Cancel_Button_Click(Me, e)
    End Sub

    Private Sub yes_button_Click(sender As Object, e As EventArgs) Handles yes_button.Click
        OK_Button_Click(Me, e)
    End Sub

    Private Sub no_button_Click(sender As Object, e As EventArgs) Handles no_button.Click

        Cancel_Button_Click(Me, e)
    End Sub

    'change button image
    Private Sub no_button_mouse_down(sender As Object, e As EventArgs) Handles no_button.MouseDown
        no.Image = Image.FromFile(".\source\button_foc.png")
        yes.Image = Image.FromFile(".\source\button_lose.png")
        'no_button.ForeColor = Color.Blue
    End Sub
End Class
