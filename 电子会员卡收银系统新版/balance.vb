Imports MySql.Data.MySqlClient
Public Class balance


    Public Shared NumId As String
    Public Shared Max As Double = 0
    Public Shared score As Double = 0 'this value be setted by cash windows
    'Public oldPMP As Double = 0
    Public Shared flag As Boolean = False  'this value be  setted by IDscan window   判断是否为会员
    Public Shared user_id As String
    Public Shared user_name As String
    Private enterflag As Boolean = False '标记是否第二次按下回车
    Private changesubed As Double = 0


    Private Sub balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.FromArgb(&HFFF7F7F7)
        balan.Parent = column
        ALL_M_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        VIP_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        PA_M_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        AC_PA_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        PA_B_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        ALL_M_ZI.BackColor = Color.FromArgb(&HFFF4F4F4)
        ' MsgBox(score.ToString)
    End Sub

    Private Sub AC_P_I_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles AC_P_I.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or e.KeyChar = Chr(13) Or e.KeyChar = Chr(8) Then
        Else
            e.Handled = True
        End If
        If e.KeyChar = Chr(13) Then
            If Not AC_P_I.Text = "" Then
                Yes_Click(Me, e)
            End If
        End If
    End Sub

    Private Function exchange()
        Dim ACPNUM As Double = Double.Parse(AC_P_I.Text.ToString())
        Dim VIPPPNUM As Double = Double.Parse(VIP_M_P.Text.ToString())
        If flag = True Then
            Dim temp As Integer = Int(Double.Parse(VIP_M_P.Text.ToString()))
            Dim subchange As Double = System.Math.Round(Double.Parse(VIP_M_P.Text.ToString()) - temp, 2) '记录需要交费中的零钱
            If Double.Parse(AC_P_I.Text) >= Double.Parse(VIP_M_P.Text) Or Double.Parse(AC_P_I.Text) + subchange >= Double.Parse(VIP_M_P.Text) And Double.Parse(Pack_M.Text) >= subchange Then
                If ((Double.Parse(AC_P_I.Text) > Double.Parse(VIP_M_P.Text)) And (Double.Parse(AC_P_I.Text) - Double.Parse(VIP_M_P.Text) < 0.5)) Then
                    Login.MsgboxNotice("请用户以0.5元增大或减小付款金额,多的零钱无法充值", "错误", False, False, Nothing, Me, True, False)
                    exchange = False
                    Exit Function
                End If
                Dim money As Integer = Int(Double.Parse(AC_P_I.Text))
                Dim submoney As Double = System.Math.Round(Double.Parse(AC_P_I.Text) - money, 2)  '得到客户给收银员中的钱中的零钱
                PA_BACK_P.Text = money - temp '得到补给客户的钱
                If subchange - submoney >= 0 And Double.Parse(Pack_M.Text) >= (subchange - submoney) Then
                    subchange -= submoney
                    Pack_M.Text = Double.Parse(Pack_M.Text) - subchange
                    changesubed = subchange
                    exchange = True
                    Exit Function
                ElseIf subchange > 0.5 And Double.Parse(Pack_M.Text) >= subchange - 0.5 And Double.Parse(AC_P_I.Text) - temp >= 0.5 Then
                    subchange -= 0.5
                    Pack_M.Text = Double.Parse(Pack_M.Text) - subchange
                    changesubed = subchange
                    PA_BACK_P.Text = System.Math.Round(Double.Parse(AC_P_I.Text) - temp - 0.5, 2)
                    exchange = True
                    Exit Function
                Else
                    PA_BACK_P.Text = System.Math.Round(Double.Parse(AC_P_I.Text) - Double.Parse(VIP_M_P.Text.ToString()), 2)
                    exchange = True
                    Exit Function
                End If
            Else
                Login.MsgboxNotice("付款金额不足！", "警告", False, False, Nothing, Me, False, False)
                exchange = False
                Exit Function
            End If
        Else
            Dim payback As Double = Double.Parse(AC_P_I.Text) - Double.Parse(ALL_M_P.Text)
            If payback >= 0 Then
                PA_BACK_P.Text = System.Math.Round(payback, 2)
                exchange = True
                Exit Function
            Else
                Login.MsgboxNotice("付款金额不足！", "警告", False, False, Nothing, Me, False, False)
                exchange = False
                Exit Function
            End If
        End If
        exchange = False
    End Function

    Private Sub infowritetosql()
        If flag = True Then
            Try
                If update_user_data() Then
                    Login.MsgboxNotice("已付款成功" + vbCrLf + "扣除零钱：" + changesubed.ToString() + "元" + vbCrLf + "找回：" + PA_BACK_P.Text + "元", "提示", False, False, Nothing, Me, True, True)
                    destory()
                    Me.Hide()
                    background.Hide()
                Else
                    Login.write_errmsg("未完成付款，请尝试重启收银台系统解决...", Me.Name, "writetosql", Me)
                End If
            Catch ex As Exception
                Login.write_errmsg(ex.Message, Me.Name, "writetosql", Me)
            End Try
        End If
    End Sub

    '更新库存
    Private Sub writestock()
        Dim str As String
        Dim dr As SQLite.SQLiteCommand
        Dim table As New DataTable
        For i = 0 To cash.Data.Rows.Count - 1
            table.Reset()
            str = "select goods_id from goods where shop_id = " & Login.shopID.ToString() & " and code = '" & cash.Data.Rows(i).Cells(1).Value.ToString & "';"
            Dim da As New SQLite.SQLiteDataAdapter(str, Login.sqliteconn)
            da.Fill(table)
            str = "insert into sync(shop_id,user_id,column,operate,value) values(" & Login.shopID.ToString() & "," & table.Rows.Item(0).Item(0) & ",'" & "stock" & "'," & "'-'," & cash.Data.Rows(i).Cells(4).Value.ToString & ");"
            dr = New SQLite.SQLiteCommand(str, Login.sqliteconn)
            dr.ExecuteNonQuery()
        Next
    End Sub

    '更新用户零钱和总体消费以及积分
    Private Function update_user_data() '这里的异常可能会导致数据不一致性问题
        Try

            Dim SC As String = score.ToString()
            Dim Num As String = NumId.ToString()
            Dim str As String = "begin transaction"            
            Dim Dr As SQLite.SQLiteCommand = New SQLite.SQLiteCommand(str, Login.sqliteconn)
            Dr.CommandType = CommandType.Text
            Dr.ExecuteNonQuery()    '开始事务
            str = "insert into sync(shop_id,user_id,column,operate,value) values(" & Login.shopID.ToString() & "," & Num & ",'" & "score" & "'," & "'+'," & score & ");"
            Dr = New SQLite.SQLiteCommand(str, Login.sqliteconn)
            Dr.ExecuteNonQuery()
            str = "insert into sync(shop_id,user_id,column,operate,value) values(" & Login.shopID.ToString() & "," & Num & ",'" & "totalspend" & "'," & "'+'," & VIP_M_P.Text.ToString() & ");"
            Dr = New SQLite.SQLiteCommand(str, Login.sqliteconn)
            Dr.ExecuteNonQuery()
            If Not changesubed = 0 Then
                str = "insert into sync(shop_id,user_id,column,operate,value) values(" & Login.shopID.ToString() & "," & Num & ",'" & "balance" & "'," & "'-'," & changesubed.ToString() & ");"
                Dr = New SQLite.SQLiteCommand(str, Login.sqliteconn)
                Dr.ExecuteNonQuery()
            End If
            writestock()
            str = "commit transaction"
            Dr = New SQLite.SQLiteCommand(str, Login.sqliteconn)
            Dr.ExecuteNonQuery() '结束事务
            update_user_data = True
        Catch ex As Exception
            Login.write_errmsg(ex.Message, Me.Name, "update_user_data", Me)
            update_user_data = False
        End Try
    End Function


    Private Sub Yes_Click(sender As Object, e As EventArgs) Handles Yes.Click
        If enterflag = False Then
            If exchange() = True Then
                enterflag = True
            End If
        Else
            If flag = True Then
                infowritetosql()
            Else
                writestock()
                Login.MsgboxNotice("已付款成功！" + vbCrLf + "找回：" + PA_BACK_P.Text + " 元", "消息", False, False, Nothing, Me, True, True)
                destory()
                Me.Hide()
                background.Hide()
            End If
        End If
    End Sub

    Private Sub destory()
        cash.lineNum = 1
        For i = 0 To cash.Data.Rows.Count - 1
            cash.Data.Rows.RemoveAt(0)
        Next
        AC_P_I.Text = ""
        ALL_M_P.Text = "0"
        VIP_M_P.Text = "0"
        Pack_M.Text = "0"
        PA_BACK_P.Text = "0"
        flag = False
        enterflag = False
        cash.ALL_M_P.Text = ""
        cash.ALL_N_P.Text = ""
        cash.scoretable.reset()
    End Sub


    'Private Sub subStock()
    '    Dim str As String
    '    For i = 0 To cash.Data.Rows.Count - 1
    '        Try
    '            str = "update goods set stock = stock - " + cash.Data.Rows(0).Cells(4).Value.ToString + " where id = " + cash.Data.Rows(0).Cells(1).Value.ToString
    '            Dim Dr As SQLite.SQLiteCommand = New SQLite.SQLiteCommand(str, Login.sqliteconn)
    '            Dr.CommandType = CommandType.Text
    '            Dr.ExecuteNonQuery()
    '        Catch ex As Exception
    '            'Login.write_errmsg(ex.Message, Me.Name, "subStock", Me)
    '            BeginInvoke(New Login.write_err_msg(AddressOf Login.Write_Err_Msg_Invo), ex.Message, Me.Name, "subStock", Me)
    '        End Try
    '    Next
    '    Me.Close()
    'End Sub


    Private Sub me_key(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Hide()
            IDScan.Show(background)
            IDScan.ID_I.Text = user_id
            IDScan.User_name.Text = user_name
            AC_P_I.Text = ""
            ALL_M_P.Text = "0"
            VIP_M_P.Text = "0"
            Pack_M.Text = "0"
            PA_BACK_P.Text = "0"
            flag = False
            cash.ALL_M_P.Text = ""
            cash.ALL_N_P.Text = ""
        End If
        If e.KeyCode = Keys.F4 Then
            AC_P_I.Text = ""
            PA_BACK_P.Text = "0"
        End If
    End Sub



End Class