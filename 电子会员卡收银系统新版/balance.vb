Imports MySql.Data.MySqlClient
Public Class balance


    Public NumId As String
    Public Max As Double = 0
    Public score As Double = 0 'this value be setted by cash windows
    'Public oldPMP As Double = 0
    Public flag As Boolean = False  'this value be  setted by IDscan window


    Private Sub balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.FromArgb(&HFFF7F7F7)
        balan.Parent = column
        ALL_M_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        VIP_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        PA_M_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        AC_PA_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        PA_B_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        ALL_M_ZI.BackColor = Color.FromArgb(&HFFF4F4F4)
        MsgBox(score.ToString)
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles AC_P_I.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or e.KeyChar = Chr(13) Or e.KeyChar = Chr(8) Then
        Else
            e.Handled = True
        End If
        If e.KeyChar = Chr(13) Then
            If AC_P_I.Text = "" Then
            Else
                Yes_Click(Me, e)
            End If
        End If
    End Sub

    Private Function exchange()
        Dim ACPNUM As Double = Double.Parse(AC_P_I.Text.ToString())
        Dim VIPPPNUM As Double = Double.Parse(VIP_M_P.Text.ToString())
        If flag = True Then
            'If ACPNUM - Double.Parse(VIP_M_P.Text.ToString()) >= 0 Then
            '    If oldPMP + ACPNUM - VIPPPNUM <= Max Then
            '        Pack_M.Text = oldPMP + ACPNUM - VIPPPNUM
            '        Pack_M.Text = "0"
            '        exchange = True
            '    Else
            '        Dim mon As Integer
            '        mon = oldPMP + ACPNUM - VIPPPNUM - Max + 1
            '        Pack_M.Text = ACPNUM - VIPPPNUM - mon
            '        Pack_M.Text = mon
            '        exchange = True
            '    End If
            'Else
            '    If ACPNUM + oldPMP - VIPPPNUM >= 0 Then
            '        Pack_M.Text = ACPNUM + oldPMP - VIPPPNUM
            '        exchange = True
            '    Else
            '        exchange = False
            '        Dim form As New MSG
            '        form.head.Text = "警告"
            '        form.msgP.Text = "付款金额不足！"
            '        form.Show()
            '        AC_P_I.Text = ""
            '        'Message.Msg.Text = "付款金额不足！"
            '        'Message.Show()
            '    End If

            'End If
            Dim temp As Integer = Int(Double.Parse(VIP_M_P.Text.ToString()))
            Dim subchange As Double = Double.Parse(VIP_M_P.Text.ToString()) - temp '记录需要交费中的零钱
            'MsgBox(Double.Parse(VIP_M_P.Text.ToString()) - temp)
            If Double.Parse(AC_P_I.Text) >= Double.Parse(VIP_M_P.Text) Or Double.Parse(AC_P_I.Text) + subchange >= Double.Parse(VIP_M_P.Text) And Double.Parse(Pack_M.Text) >= subchange Then
                If ((Double.Parse(AC_P_I.Text) > Double.Parse(VIP_M_P.Text)) And (Double.Parse(AC_P_I.Text) - Double.Parse(VIP_M_P.Text) < 0.5)) Then
                    Login.MsgboxNotice("请用户以0.5元增大或减小付款金额,多的零钱无法充值", "错误", False, True, Nothing, Me)
                    exchange = False
                    Exit Function
                End If
                Dim money As Integer = Int(Double.Parse(AC_P_I.Text))
                Dim submoney As Double = Double.Parse(AC_P_I.Text) - money  '得到客户给收银员中的钱中的零钱
                PA_BACK_P.Text = money - temp '得到补给客户的钱
                subchange -= submoney
                Pack_M.Text = Double.Parse(Pack_M.Text) - subchange
                exchange = True
            Else


                exchange = False
                Login.MsgboxNotice("付款金额不足！", "警告", False, True, Nothing, Me)
            End If
            'MsgBox(subchange)
        End If
        exchange = False
    End Function

    Private Sub writetosql()
        If flag = True Then
            Try
                Dim SC As String = score.ToString()
                Dim Num As String = NumId.ToString()
                Dim str As String = "update utos set balance = " + Pack_M.Text.ToString() + ", score = " + SC + " where user_id = '" + Num + "'"
                Dim Dr As SQLite.SQLiteCommand = New SQLite.SQLiteCommand(str, Login.sqliteconn)
                Dr.CommandType = CommandType.Text
                Dr.ExecuteNonQuery()
                'Dim msgform As New MSG
                'msgform.Text = "提示"
                'msgform.msgP.Text = "已付款成功"
                'msgform.Show()
                Login.MsgboxNotice("已付款成功", "提示", False, True, Nothing, Me)
                'subStock()
                destory()
                Me.Close()
                background.Hide()
            Catch ex As Exception
                Login.write_errmsg(ex.Message, Me.Name, "writetosql", Me)
            End Try
        End If
    End Sub


    Private Sub Yes_Click(sender As Object, e As EventArgs) Handles Yes.Click
        If exchange() = True Then
            writetosql()
        End If
    End Sub

    Private Sub destory()
        cash.lineNum = 1
        For i = 0 To cash.Data.Rows.Count - 1
            cash.Data.Rows.RemoveAt(0)
        Next
        cash.ALL_M_P.Text = ""
        cash.ALL_N_P.Text = ""
    End Sub


    Private Sub subStock()
        Dim str As String
        For i = 0 To cash.Data.Rows.Count - 1
            Try
                str = "update goods set stock = stock - " + cash.Data.Rows(0).Cells(4).Value.ToString + " where id = " + cash.Data.Rows(0).Cells(1).Value.ToString
                Dim Dr As SQLite.SQLiteCommand = New SQLite.SQLiteCommand(str, Login.sqliteconn)
                Dr.CommandType = CommandType.Text
                Dr.ExecuteNonQuery()
            Catch ex As Exception
                Login.write_errmsg(ex.Message, Me.Name, "subStock", Me)
            End Try
        Next
        Me.Close()
    End Sub

End Class