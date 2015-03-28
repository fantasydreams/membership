Imports MySql.Data.MySqlClient

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
                    balance.ALL_M_P.Text = cash.ALL_M_P.Text
                    balance.VIP_M_P.Text = balance.ALL_M_P.Text
                    balance.Pack_M.Text = "0"
                    balance.Show()
                Else
                    If sureUserInfo = True Then
                        CalculateVipMon()
                        Me.Close()
                        balance.Show()
                    End If
                    If sureUserInfo = False Then

                        If chooseDatabase() = True Then
                            getInfoFromDatabase()
                            If sureUserInfo = False Then
                                sureUserInfo = True
                            End If
                        Else
                            Dim form As New MSG
                            form.head.Text = "error"
                            form.msgP.Text = "没有此会员信息！"
                            form.Show()
                            ID_I.Text = ""
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
        For i = 0 To cash.Data.Rows.Count - 1
            Dim str As String = "select discount from goods where code = '" + cash.Data.Rows(i).Cells(1).Value.ToString + "' and shop_id = " + Login.shopID.ToString
            Dim Dr As MySqlCommand = New MySqlCommand(str, Login.conn)
            Dr.CommandType = CommandType.Text
            Dim MR As MySqlDataReader
            MR = Dr.ExecuteReader()
            If MR.HasRows Then
                MR.Read()
                Money += Double.Parse(cash.Data.Rows(i).Cells(6).Value.ToString()) * Double.Parse(MR.Item(0).ToString())
            Else
                Money += Double.Parse(cash.Data.Rows(i).Cells(6).Value.ToString())
            End If
            MR.Close()
        Next
        balance.VIP_M_P.Text = Money
        balance.ALL_M_P.Text = cash.ALL_M_P.Text
        balance.flag = True
    End Sub


    Private Function chooseDatabase()
        Dim str As String = "select balance,changesLimt,score from utos where user_id = " + ID_I.Text
        Dim Dr As MySqlCommand = New MySqlCommand(str, Login.conn)
        Dr.CommandType = CommandType.Text
        Try
            Dim MR As MySqlDataReader
            MR = Dr.ExecuteReader()
            If MR.HasRows Then
                MR.Read()
                balance.Max = MR.Item(1)
                balance.score = Integer.Parse(MR.Item(2).ToString) + Integer.Parse(balance.ALL_M_P.Text)
                balance.Pack_M.Text = MR.Item(0)
                balance.oldPMP = Double.Parse(MR.Item(0))
                balance.NumId = ID_I.Text.ToString()
                MR.Close()
                chooseDatabase = True
            Else
                MR.Close()
                str = "select id from user where id = " + ID_I.Text.ToString
                Dim CR As MySqlCommand = New MySqlCommand(str, Login.conn)
                Dim MC As MySqlDataReader
                MC = CR.ExecuteReader()
                If MC.HasRows Then
                    MC.Close()
                    str = "insert into utos (user_id,shop_id) values(" + ID_I.Text.ToString + "," + Login.shopID.ToString + ");"
                    Dim D As MySqlCommand = New MySqlCommand(str, Login.conn)
                    D.CommandType = CommandType.Text
                    D.ExecuteNonQuery()
                    Dim form As New MSG
                    form.head.Text = "提示"
                    form.msgP.Text = "会员未完善信息，请提醒会员完善信息..."
                    form.Show()
                    GetUserInfoFlag = True
                    chooseDatabase = True
                Else
                    MC.Close()
                    chooseDatabase = False
                End If

            End If
        Catch ex As Exception
            chooseDatabase = False
            MsgBox(ex.ToString)
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
            Dim Dr As MySqlCommand = New MySqlCommand(str, Login.conn)
            Dr.CommandType = CommandType.Text
            Dim MR As MySqlDataReader
            MR = Dr.ExecuteReader()
            If MR.HasRows Then
                MR.Read()
                User_name.Text = MR.Item(0).ToString
                GetUserInfoFlag = True
                MR.Close()
                Exit Sub
            Else
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
End Class