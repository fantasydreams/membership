Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Public Class IDScan
    Dim sureUserInfo As Boolean = False
    Dim GetUserInfoFlag As Boolean = False
    Private Sub IDScan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.FromArgb(&HFFF7F7F7)
        ID_SCAN_ZI.Parent = column
        PA_B_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        Count_B.BackColor = Color.FromArgb(&HFFC5C5C5)
    End Sub

    Private Sub ID_I_keyPress(sender As Object, e As KeyPressEventArgs) Handles ID_I.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or e.KeyChar = Chr(13) Or e.KeyChar = Chr(Keys.Back) Then
            If e.KeyChar = Chr(Keys.Back) Then
                sureUserInfo = False
            End If
            If e.KeyChar = ChrW(13) Then
                If ID_I.Text = "" Then
                    Me.Close()
                    'balance.ALL_M_P.Text = cash.ALL_M_P.Text
                    'balance.VIP_M_P.Text = balance.ALL_M_P.Text
                    'balance.Pack_M.Text = "0"
                    BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), balance.ALL_M_P, cash.ALL_M_P.Text)
                    BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), balance.VIP_M_P, balance.ALL_M_P.Text)
                    BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), balance.Pack_M, "0")
                    balance.Show(background)
                Else
                    If sureUserInfo = True Then
                        CalculateVipMon()
                        balance.user_id = ID_I.Text
                        balance.user_name = User_name.Text
                        Me.Close()
                        balance.Show(background)
                    End If
                    If sureUserInfo = False Then

                        If chooseDatabase() = True Then
                            getInfoFromDatabase()
                            If sureUserInfo = False Then
                                sureUserInfo = True
                            End If
                        Else
                            'Login.MsgboxNotice("没有此会员信息！", "消息", False, False, Nothing, Me, True, False)
                            BeginInvoke(New Login.msgbox_de(AddressOf Login.msgbox_invo), "没有此会员信息！", "消息", False, False, Nothing, Me, True, False)
                            'ID_I.Text = ""
                            BeginInvoke(New Login.TextBox_text(AddressOf Login.TextBox_text_invo), ID_I.Text, "")
                        End If

                    End If
                End If
            End If
        Else
            e.Handled = True
        End If
    End Sub

    'calculate vip should pay
    Public Sub CalculateVipMon()
        Dim Money As Double = 0
        Dim TableData As New DataTable
        Dim sqlda As SQLite.SQLiteDataAdapter
        For i = 0 To cash.Data.Rows.Count - 1
            Dim str As String = "select discount from goods where code = '" + cash.Data.Rows(i).Cells(1).Value.ToString + "' and shop_id = " + Login.shopID.ToString
            TableData.Reset()
            sqlda = New SQLite.SQLiteDataAdapter(str, Login.sqliteconn)
            sqlda.Fill(TableData)
            If TableData.Rows.Count() Then
                Money += Double.Parse(cash.Data.Rows(i).Cells(6).Value.ToString()) * Double.Parse(TableData.Rows.Item(0).Item(0).ToString())
            Else
                Money += Double.Parse(cash.Data.Rows(i).Cells(6).Value.ToString())
            End If
        Next
        'balance.VIP_M_P.Text = Money
        'balance.ALL_M_P.Text = cash.ALL_M_P.Text
        BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), balance.VIP_M_P, Money.ToString)
        BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), balance.ALL_M_P, cash.ALL_M_P.Text)
        balance.flag = True
    End Sub


    Private Function chooseDatabase()
        Dim str As String = "select balance from utos where user_id = " + ID_I.Text
        Dim sqliteadpater As SQLite.SQLiteDataAdapter
        Dim tableData As New DataTable
        Try
            sqliteadpater = New SQLite.SQLiteDataAdapter(str, Login.sqliteconn)
            tableData.Reset()
            sqliteadpater.Fill(tableData)
            If tableData.Rows.Count Then
                'balance.Max = tableData.Rows.Item(0).Item(1)
                'balance.score = Double.Parse(MR.Item(2).ToString) + cash.score
                '积分

                'balance.Pack_M.Text = tableData.Rows.Item(0).Item(0)
                BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), balance.Pack_M, tableData.Rows.Item(0).Item(0).ToString)
                'balance.oldPMP = Double.Parse(tableData.Rows.Item(0).Item(0))
                balance.NumId = ID_I.Text.ToString()
                chooseDatabase = True
            Else
                str = "select id from user where id = " + ID_I.Text.ToString
                sqliteadpater = New SQLite.SQLiteDataAdapter(str, Login.sqliteconn)
                tableData.Reset()
                sqliteadpater.Fill(tableData)
                If tableData.Rows.Count Then
                    str = "insert into utos (user_id,shop_id) values(" + ID_I.Text.ToString + "," + Login.shopID.ToString + ");"
                    Dim sqlcom As SQLite.SQLiteCommand = New SQLite.SQLiteCommand(str, Login.sqliteconn)
                    sqlcom.CommandType = CommandType.Text
                    sqlcom.ExecuteNonQuery()
                    'Dim form As New MSG
                    'form.head.Text = "提示"
                    'form.msgP.Text = "会员未完善信息，请提醒会员完善信息..."
                    'form.Show()
                    balance.NumId = ID_I.Text.ToString()
                    ' Login.MsgboxNotice("会员未完善信息" + vbCrLf + "请提醒会员完善信息...", "提示", False, False, Nothing, Me, True, False)
                    BeginInvoke(New Login.msgbox_de(AddressOf Login.msgbox_invo), "会员未完善信息" + vbCrLf + "请提醒会员完善信息...", "提示", False, False, Nothing, Me, True, False)
                    GetUserInfoFlag = True
                    chooseDatabase = True
                Else
                    chooseDatabase = False
                End If
            End If
        Catch ex As Exception
            chooseDatabase = False
            'Login.write_errmsg(ex.Message, "发生了一个错误", "chooseDatabase", Me)
            BeginInvoke(New Login.write_err_msg(AddressOf Login.Write_Err_Msg_Invo), ex.Message, "发生了一个错误", "chooseDatabase", Me)
        End Try
    End Function


    'Private Sub getUserInfo()
    '    If Login.conn.State = ConnectionState.Broken Then
    '        If Login.ConcectDataIfBreak() Then
    '            getInfoFromDatabase()
    '        Else
    '            '这里填写错误提示信息


    '        End If
    '    Else
    '        getInfoFromDatabase()
    '    End If
    'End Sub


    Private Sub getInfoFromDatabase()
        Dim str As String = "select name from userinfo where user_id = " + ID_I.Text.ToString()
        ' MsgBox(str)
        Try
            'Dim Dr As MySqlCommand = New MySqlCommand(str, Login.conn)
            'Dr.CommandType = CommandType.Text
            'Dim MR As MySqlDataReader
            'MR = Dr.ExecuteReader()
            Dim sqliteadapter As New SQLite.SQLiteDataAdapter(str, Login.sqliteconn)
            Dim tabledata As New DataTable
            tabledata.Reset()
            sqliteadapter.Fill(tabledata)
            If tabledata.Rows.Count Then
                If Not tabledata.Rows.Item(0).Item(0).ToString = "" Then
                    'User_name.Text = tabledata.Rows.Item(0).Item(0).ToString
                    BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), User_name, tabledata.Rows.Item(0).Item(0).ToString)
                    GetUserInfoFlag = True
                    Exit Sub
                Else
                    'Login.MsgboxNotice("此会员没有完善信息！提醒会员完善信息！", "消息", False, False, Nothing, Me, True, False)
                    BeginInvoke(New Login.msgbox_de(AddressOf Login.msgbox_invo), "会员未完善信息" + vbCrLf + "请提醒会员完善信息...", "提示", False, False, Nothing, Me, True, False)
                    GetUserInfoFlag = True
                End If
                
            Else
                'Login.MsgboxNotice("此会员没有完善信息！提醒会员完善信息！", "消息", False, False, Nothing, Me, True, False)
                BeginInvoke(New Login.msgbox_de(AddressOf Login.msgbox_invo), "会员未完善信息" + vbCrLf + "请提醒会员完善信息...", "提示", False, False, Nothing, Me, True, False)
                GetUserInfoFlag = True
                '这里填写未找到相关会员的信息
                'MsgBox("未找到相关会员！")
                'MR.Close()
                'str = "select id from user where id = " + ID_I.Text.ToString
                'Dim CR As MySqlCommand = New MySqlCommand(str, Login.conn)
                'Dim MC As MySqlDataReader
                'MC = CR.ExecuteReader()
                'If MC.HasRows Then
                '    Dim form As New MSG
                '    form.head.Text = "提示"
                '    form.msgP.Text = "会员未完善信息，请提醒会员完善信息..."
                '    form.Show()
                '    GetUserInfoFlag = True
                'Else
                '    Dim form As New MSG
                '    form.head.Text = "提示"
                '    form.msgP.Text = "没有找到相关会员..."
                '    form.Show()
                '    GetUserInfoFlag = False
                'End If
                'MC.Close()
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString)
            'Login.write_errmsg(ex.Message, Me.Name, "getInfoFromDatabase", Me)
            BeginInvoke(New Login.write_err_msg(AddressOf Login.Write_Err_Msg_Invo), ex.Message, Me.Name, "getInfoFromDatabase", Me)
        End Try
    End Sub

    'Private Sub ID_I_TextChanged(sender As Object, e As KeyPressEventArgs) Handles ID_I.KeyPress
    '    If e.KeyChar = ChrW(13) Then
    '        If ID_I.Text = "" Then
    '        Else
    '            If GetUserInfoFlag = False Then
    '                getUserInfo()
    '            Else
    '                Me.Hide()
    '                balance.Show()
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub ID_I_TextChanged(sender As Object, e As EventArgs) Handles ID_I.TextChanged
        If Not User_name.Text = "" Then
            'User_name.Text = ""
            BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), User_name, "")
        End If
    End Sub

    Private Sub me_key(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            background.Close()
        End If
        If e.KeyCode = Keys.F4 Then
            'ID_I.Text = ""
            BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), ID_I, "")
        End If
    End Sub

End Class