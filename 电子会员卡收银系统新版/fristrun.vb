Public Class fristrun

    Private Sub fristrun_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.BackColor = Color.White
        Yes_Button.Parent = Yes_pic
    End Sub

    '控制输入框的背景颜色
    Private Sub shop_id_in_GotFocus(sender As Object, e As EventArgs) Handles shop_id_in.GotFocus
        In_back0.BackColor = Color.FromArgb(&HFF569DE5)
    End Sub
    Private Sub shop_id_in_LostFocus(sender As Object, e As EventArgs) Handles shop_id_in.LostFocus
        In_back0.BackColor = Color.DimGray
    End Sub


    '只允许输入数组和回车与退格键
    Private Sub shop_id_in_keypress(sender As Object, e As KeyPressEventArgs) Handles shop_id_in.KeyPress
        If Not (e.KeyChar >= "0" And e.KeyChar <= "9" Or e.KeyChar = Chr(Keys.Enter) Or e.KeyChar = Chr(Keys.Back)) Then
            e.Handled = True
        End If
    End Sub

    '不处理选择框中的任何输入
    Private Sub shop_list_SelectedIndexChanged(sender As Object, e As KeyPressEventArgs) Handles shop_list.KeyPress
        e.Handled = True
    End Sub
End Class