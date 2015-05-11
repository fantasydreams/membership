
Imports MySql.Data.MySqlClient
Public Class cash
    'Public score As Double = 0  'shop goods score
    Public Shared lineNum As Integer = 1
    'score table
    Public Shared scoreTable As New DataTable
    'Private cancleFlag(21) As Integer '用来记录上次的操作的情况
    Delegate Sub windows_load()
    Private Sub thread(func As String)
        Dim func_thread As Threading.Thread
        Select Case func
            Case "calculatescore"
                func_thread = New Threading.Thread(AddressOf calculatescore)
            Case "selectFromBaseData"
                func_thread = New Threading.Thread(AddressOf selectFromBaseData)
            Case "clear"
                func_thread = New Threading.Thread(AddressOf clear)
            Case "cancleLastStep"
                func_thread = New Threading.Thread(AddressOf cancleLastStep)
            Case Else
                Exit Sub
        End Select
        func_thread.Start()
    End Sub
    Private Sub windowsLoad_invo()
        'Me.SuspendLayout()
        Me.SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        Me.Size = New Size(Login.ScreenWidth, Login.ScreenHeight)
        Me.Location = New Point(0, 0)
        'MsgBox(Login.ScreenWidth.ToString + "    " + Login.ScreenHeight.ToString)
        Me.BackColor = Color.FromArgb(&HFFF1F1F1)
        'Me.Visible = True

        column.Size = New Size(Login.ScreenWidth, 40)
        'column.Location = New Point(0, 0)

        Esc.Parent = column
        F4.Parent = column
        F5.Parent = column
        Cancellation.Parent = column
        about.Parent = column

        'column1
        'column1.Location = New Point(0, 136)
        column1.Size = New Size(Login.ScreenWidth, 40)

        Line_num.Parent = column1
        Buy_ID.Parent = column1
        P_Name_.Parent = column1
        p_Unit.Parent = column1
        P_NUM.Parent = column1
        P_Price.Parent = column1
        P_Mon.Parent = column1


        ALL_M_Back.BackColor = Color.FromArgb(&HFFD7D7D7)
        ALL_M_Back.Size = New Size(230, 65)
        ALL_M_Back.Location = New Point(Me.Width - ALL_M_Back.Width - 10, 56)
        'Me.BackColor = Color.Red

        ALLN_Back.BackColor = Color.FromArgb(&HFFD7D7D7)
        ALLN_Back.Size = ALL_M_Back.Size
        ALLN_Back.Location = New Point(ALL_M_Back.Left - ALL_M_Back.Width - 10, 56)


        ID_Back.BackColor = Color.FromArgb(&HFFD7D7D7)
        ID_Back.Size = New Size(ALLN_Back.Left - 15 - ID_Back.Left, 65)

        back_IS.Size = New Size(ID_Back.Width - 6, ID_Back.Height - 4)


        ALL_NUM.Location = New Point(ALLN_Back.Left + 2, ALLN_Back.Top + 2)
        ALL_NUM.Size = New Size(ALL_M_Back.Width - 4, ALL_M_Back.Height - 4)
        ALL_NUM.BackColor = Color.FromArgb(&HFFF1F1F1)

        ALL_Money.BackColor = ALL_NUM.BackColor
        ALL_Money.Size = ALL_NUM.Size
        ALL_Money.Location = New Point(ALL_M_Back.Left + 2, ALL_M_Back.Top + 2)

        ID_P_A_I.BackColor = Color.White
        ID_P_A_I.Location = New Point(back_IS.Left, back_IS.Top + 13)
        ID_P_A_I.Size = New Size(back_IS.Width, ID_P_A_I.Height)

        ' ALL_NUM_zi.Parent = ALLN_Back
        ALL_NUM_zi.Size = New Size(90, ALLN_Back.Height - 4)
        ALL_NUM_zi.Location = New Point(ALLN_Back.Left + 2, ALLN_Back.Top + 2)


        SHU.Location = New Point(ALL_NUM_zi.Left + ALL_NUM_zi.Width, ALLN_Back.Top)
        SHU.Size = New Size(2, ALLN_Back.Height)
        SHU.BackColor = Color.FromArgb(&HFFD7D7D7)

        ALL_N_P.Location = New Point(SHU.Left + 2, ALL_NUM_zi.Top)
        ALL_N_P.Size = New Size(ALLN_Back.Width - ALL_NUM_zi.Width - 6, ALL_NUM_zi.Height)
        ALL_N_P.Text = "0"


        ALL_M_ZI.Size = ALL_NUM_zi.Size
        ALL_M_ZI.Location = New Point(ALL_M_Back.Left + 2, ALL_NUM_zi.Top)

        SHU1.Size = SHU.Size
        SHU1.Location = New Point(ALL_M_ZI.Left + ALL_M_ZI.Width, ALL_M_Back.Top)
        SHU1.BackColor = SHU.BackColor

        ALL_M_P.Size = ALL_N_P.Size
        ALL_M_P.Location = New Point(SHU1.Left + 2, SHU1.Top + 2)
        ALL_M_P.Text = "0"

        Line.BackColor = Color.FromArgb(&HFFD48181)
        Line.Width = Login.ScreenWidth

        Data.Size = New Size(Login.ScreenWidth, Login.ScreenHeight - 176)
        Data.BackgroundColor = Color.White
        'Data.Show()
        SetLableAndDataGridViewWith()
        Me.ResumeLayout()
        Me.PerformLayout()
    End Sub
    Public Sub cash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New windows_load(AddressOf windowsLoad_invo))
    End Sub

    Private Sub Esc_Click(sender As Object, e As EventArgs) Handles Esc.Click
        If Login.MsgboxNotice("按下enter退出系统，esc返回...", "即将退出系统", True, True, "取消", Me, True, False) = DialogResult.OK Then
            Login.sqliteconn.Close()
            End '关闭程序
        End If
    End Sub

    Private Sub F4_Click(sender As Object, e As EventArgs) Handles F4.Click
        thread("clear")
    End Sub

    '设置data以及对应的列标签的宽度
    Private Sub SetLableAndDataGridViewWith()
        Dim rate As Double = Login.ScreenWidth / 1366
        Buy_ID.Width = 366 * rate
        P_Name_.Location = New Point(Buy_ID.Width + 120, 0)
        P_Name_.Size = New Size(200 * rate, 40)

        p_Unit.Location = New Point(P_Name_.Left + P_Name_.Width, 0)
        p_Unit.Size = New Size(160 * rate, 40)

        P_NUM.Location = New Point(p_Unit.Left + p_Unit.Width, 0)
        P_NUM.Size = p_Unit.Size

        P_Price.Location = New Point(P_NUM.Left + P_NUM.Width, 0)
        P_Price.Size = P_NUM.Size

        P_Mon.Location = New Point(P_Price.Left + P_Price.Width, 0)
        P_Mon.Size = P_Name_.Size
        Data.Columns(0).Width = Line_num.Width
        Data.Columns(1).Width = Buy_ID.Width
        Data.Columns(2).Width = P_Name_.Width
        Data.Columns(3).Width = p_Unit.Width
        Data.Columns(4).Width = P_NUM.Width
        Data.Columns(5).Width = P_Price.Width
        Data.Columns(6).Width = P_Mon.Width


    End Sub

    Private Sub ID_P_A_I_TextChanged(sender As Object, e As KeyPressEventArgs) Handles ID_P_A_I.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = "." Or e.KeyChar = Chr(13) Or e.KeyChar = ChrW(8) Then
            If e.KeyChar = ChrW(13) Then
                If ID_P_A_I.Text = "" Then
                    If Not ALL_N_P.Text = "0" Then
                        background.Show(Me)
                        IDScan.Show(background)
                        thread("calculatescore")
                    End If
                Else
                    thread("selectFromBaseData")
                    'selectData()
                    'selectFromBaseData()
                End If
            End If
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub text_box_text(mysqlTextBox As TextBox, str As String)
        mysqlTextBox.Text = str
    End Sub

    '重新计算总额
    Private Sub changeMoney()
        Dim money As Double = 0
        For i = 0 To Data.Rows.Count - 1
            money += Double.Parse(Data.Rows(i).Cells(6).Value)
        Next
        BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), ALL_M_P, money.ToString)
    End Sub

    '重新计算商品数量
    Private Sub calculatePronum()
        Dim Num As Integer = 0
        For i = 0 To Data.Rows.Count - 1
            Num += Data.Rows(i).Cells(4).Value
        Next
        BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), ALL_N_P, Num.ToString)
        'ALL_N_P.Text = Num
    End Sub

    Private Delegate Sub setData(table As DataTable)
    Private Sub setData_data(table As DataTable)
        For i = 0 To Data.Rows.Count - 1
            If Data.Rows(i).Cells(1).Value = ID_P_A_I.Text.ToString() Then
                Data.Rows(i).Cells(4).Value += 1
                Data.Rows(i).Cells(6).Value = Double.Parse(Data.Rows(i).Cells(4).Value) * Double.Parse(Data.Rows(i).Cells(5).Value)
                Exit Sub
            End If
        Next
        Data.Rows.Add()
        Data.Rows(Data.Rows.Count - 1).Height = 45
        Data.Rows(lineNum - 1).Cells(0).Value = lineNum
        Data.Rows(lineNum - 1).Cells(1).Value = ID_P_A_I.Text.ToString()
        Data.Rows(lineNum - 1).Cells(2).Value = table.Rows.Item(0).Item(0)
        Data.Rows(lineNum - 1).Cells(4).Value = 1
        Data.Rows(lineNum - 1).Cells(5).Value = table.Rows.Item(0).Item(2)
        Data.Rows(lineNum - 1).Cells(6).Value = table.Rows.Item(0).Item(2)
        scoreTable.Rows.Add()
        scoreTable.Rows.Item(scoreTable.Rows.Count - 1).Item(0) = Double.Parse(table.Rows.Item(0).Item(3).ToString()) 'calculate goods scores
        If Data.Rows.Count Mod 2 = 0 Then
            Data.Rows(Data.Rows.Count - 1).DefaultCellStyle.BackColor = Color.FromArgb(&HFFF7F7F7)
        Else
            Data.Rows(Data.Rows.Count - 1).DefaultCellStyle.BackColor = Color.White
        End If
        lineNum += 1
    End Sub

    '从数据库得到数据
    Private Sub selectFromBaseData()
        Try
            If ID_P_A_I.Text = "" Then
            Else
                If scoreTable.Columns.Count = 0 Then
                    scoreTable.Columns.Add(1)
                End If
                Dim str As String = "select name ,shop_id,price,score from goods where code = " + ID_P_A_I.Text.ToString()
                Dim sqliteadapter As New SQLite.SQLiteDataAdapter(str, Login.sqliteconn)
                Dim table As New DataTable
                table.Reset()
                sqliteadapter.Fill(table)
                Dim temp As String = ""
                Dim flag As Boolean = False
                If table.Rows.Count() Then
                    flag = True
                    Dim result As IAsyncResult = BeginInvoke(New setData(AddressOf setData_data), table)
                    EndInvoke(result)
                    temp = table.Rows.Item(0).Item(1)
                    'ID_P_A_I.Text = ""
                    BeginInvoke(New Login.TextBox_text(AddressOf Login.TextBox_text_invo), ID_P_A_I, "")
                Else  'print the error msg
                    BeginInvoke(New Login.msgbox_de(AddressOf Login.msgbox_invo), "仓库不存在此商品", "提示", False, False, Nothing, Me, True, False)
                    'ID_P_A_I.Text = ""
                    BeginInvoke(New Login.TextBox_text(AddressOf Login.TextBox_text_invo), ID_P_A_I, "")
                End If
                If flag = True Then
                    'Data.Rows(lineNum - 2).Cells(3).Value = getshopName(temp) '名称
                    BeginInvoke(New set_data_cell_value(AddressOf set_date_cell_value_invo), lineNum - 2, 3, getshopName(temp))
                    changeMoney()
                    calculatePronum()
                End If
            End If
        Catch ex As Exception
            'Login.write_errmsg(ex.Message, Me.Name, "selectFromBaseData", Me)
            BeginInvoke(New Login.write_err_msg(AddressOf Login.Write_Err_Msg_Invo), ex.Message, Me.Name, "selectFromBaseData", Me)
        End Try
    End Sub
    '设置data某个元素的值
    Delegate Sub set_data_cell_value(line As Integer, column As Integer, value As String)
    Private Sub set_date_cell_value_invo(line As Integer, column As Integer, value As String)
        Data.Rows(line).Cells(column).Value = value
    End Sub

    '设置data背景颜色
    Delegate Sub set_data_back_color(line As Integer, color As Color)
    Private Sub set_Data_BK_color(line As Integer, color As Color)
        Data.Rows(line).DefaultCellStyle.BackColor = color
    End Sub

    'calculatescore
    Private Sub calculatescore()
        Dim score As Double = 0
        Try
            For i = 0 To scoreTable.Rows.Count - 1
                score += Math.Round(Double.Parse(scoreTable.Rows().Item(i).Item(0).ToString()), 2) * Integer.Parse(Data.Rows(i).Cells(4).Value.ToString)
            Next
            balance.score = score
        Catch ex As Exception
            ' Login.write_errmsg(ex.Message, Me.Name, "calculatescore", Me)
            BeginInvoke(New Login.write_err_msg(AddressOf Login.Write_Err_Msg_Invo), ex.Message, Me.Name, "calculatescore", Me)
        End Try
    End Sub

    '重新调整datagirdview的颜色
    Private Sub ExchageBackColor(ByVal int As Integer)
        If int >= 0 Then
            For i = int To Data.Rows.Count - 1
                If i Mod 2 <> 0 Then
                    'Data.Rows(Data.Rows.Count - 1).DefaultCellStyle.BackColor = Color.FromArgb(&HFFF7F7F7)
                    BeginInvoke(New set_data_back_color(AddressOf set_Data_BK_color), Data.Rows.Count - 1, Color.FromArgb(&HFFF7F7F7))
                Else
                    ' Data.Rows(Data.Rows.Count - 1).DefaultCellStyle.BackColor = Color.White
                    BeginInvoke(New set_data_back_color(AddressOf set_Data_BK_color), Data.Rows.Count - 1, Color.White)
                End If
            Next
        End If
    End Sub

    '得到shop的名称与id
    Private Function getshopName(ByVal Id As String)
        Dim str As String = "select name from shop where id = " + Id
        'Dim Dr As MySqlCommand = New MySqlCommand(str, Login.conn)
        'Dr.CommandType = CommandType.Text
        'Dim MR As MySqlDataReader
        'MR = Dr.ExecuteReader()
        Dim sqliteadater As New SQLite.SQLiteDataAdapter(Str, Login.sqliteconn)
        Dim table As New DataTable
        table.Reset()
        sqliteadater.Fill(table)
        If table.Rows.Count Then
            getshopName = table.Rows.Item(0).Item(0)
        Else
            getshopName = ""
        End If
    End Function

    Private Sub Data_CellContentClick(sender As Object, e As MouseEventArgs) Handles Data.MouseClick
        If Data.Rows.Count Then
            If Data.CurrentCell.ColumnIndex = 4 Then
                Select Case e.Button
                    Case Windows.Forms.MouseButtons.Left
                        'Data.Rows(Data.CurrentCell.RowIndex).Cells(4).Value += 1
                        'Data.Rows(Data.CurrentCell.RowIndex).Cells(6).Value = Double.Parse(Data.Rows(Data.CurrentCell.RowIndex).Cells(5).Value) * Double.Parse(Data.Rows(Data.CurrentCell.RowIndex).Cells(4).Value)
                        Dim result As IAsyncResult = BeginInvoke(New set_data_cell_value(AddressOf set_date_cell_value_invo), Data.CurrentCell.RowIndex, 4, (Integer.Parse(Data.Rows(Data.CurrentCell.RowIndex).Cells(4).Value) + 1).ToString)
                        EndInvoke(result)
                        result = BeginInvoke(New set_data_cell_value(AddressOf set_date_cell_value_invo), Data.CurrentCell.RowIndex, 6, (Double.Parse(Data.Rows(Data.CurrentCell.RowIndex).Cells(5).Value) * Double.Parse(Data.Rows(Data.CurrentCell.RowIndex).Cells(4).Value)).ToString)
                        EndInvoke(result)
                        changeMoney()
                        calculatePronum()
                    Case Windows.Forms.MouseButtons.Right
                        If Data.Rows(Data.CurrentCell.RowIndex).Cells(4).Value >= 2 Then
                            Dim result As IAsyncResult = BeginInvoke(New set_data_cell_value(AddressOf set_date_cell_value_invo), Data.CurrentCell.RowIndex, 4, (Integer.Parse(Data.Rows(Data.CurrentCell.RowIndex).Cells(4).Value) - 1).ToString)
                            EndInvoke(result)
                            result = BeginInvoke(New set_data_cell_value(AddressOf set_date_cell_value_invo), Data.CurrentCell.RowIndex, 6, (Double.Parse(Data.Rows(Data.CurrentCell.RowIndex).Cells(5).Value) * Double.Parse(Data.Rows(Data.CurrentCell.RowIndex).Cells(4).Value)).ToString)
                            EndInvoke(result)
                            changeMoney()
                            calculatePronum()
                            Exit Select
                        End If
                        If Data.Rows(Data.CurrentCell.RowIndex).Cells(4).Value = 1 Then
                            Dim result As IAsyncResult = BeginInvoke(New dataTable_RemoveAt(AddressOf dataTable_RemoveAt_invo), scoreTable, Data.CurrentCell.RowIndex)
                            EndInvoke(result)
                            result = BeginInvoke(New Data_RemoveAt(AddressOf Data_RemoveAt_Invo), Data.CurrentCell.RowIndex)
                            lineNum -= 1
                            EndInvoke(result)
                            If Data.Rows.Count Then
                                ExchageBackColor(Data.CurrentCell.RowIndex)
                            End If
                            changeMoney()
                            calculatePronum()
                        End If
                End Select
            End If
        End If
    End Sub
    'datatable removeAt
    Delegate Sub dataTable_RemoveAt(da As DataTable, line As Integer)
    Private Sub dataTable_RemoveAt_invo(da As DataTable, line As Integer)
        da.Rows.RemoveAt(line)
    End Sub

    'form keypress detect
    Private Sub form_keypress(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                Esc_Click(Me, Nothing)
            Case Keys.F4
                clear()
            Case Keys.F5
                cancleLastStep()
            Case Keys.F6
                Cancellation_Click(Me, Nothing)
            Case Keys.F8
                about_Click(Me, Nothing)
        End Select
    End Sub

    '重置函数
    Private Sub clear()
        'ID_P_A_I.Text = ""
        BeginInvoke(New Login.TextBox_text(AddressOf Login.TextBox_text_invo), ID_P_A_I, "")
        'ALL_N_P.Text = "0"
        'ALL_M_P.Text = "0"
        BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), ALL_N_P, "0")
        BeginInvoke(New Login.Label_text(AddressOf Login.Label_text_invo), ALL_M_P, "0")
        For i = Data.Rows.Count - 1 To 0 Step -1
            If i < 0 Then
                Exit For
            End If
            BeginInvoke(New Data_RemoveAt(AddressOf Data_RemoveAt_Invo), Data.Rows.Count() - 1)
        Next i
        lineNum = 1
        scoreTable.Reset()
    End Sub

    Private Sub cancleLastStep()
        'Try
        '    If cancleFlag(0) > 0 Then
        '        Select Case cancleFlag(cancleFlag(0))
        '            Case 0
        '                If Integer.Parse(Data.Rows(lineNum - 2).Cells(4).Value) > 1 Then
        '                    Data.Rows(lineNum - 2).Cells(4).Value -= 1
        '                    Data.Rows(lineNum - 2).Cells(6).Value -= Data.Rows(lineNum - 1).Cells(5).Value
        '                    'changeMoney()
        '                    ALL_M_P.Text = Double.Parse(ALL_M_P.Text) - Double.Parse(Data.Rows(lineNum - 2).Cells(5).Value)
        '                    ALL_N_P.Text = Integer.Parse(ALL_N_P.Text) - 1
        '                Else
        '                    Data.Rows.RemoveAt(lineNum - 2)
        '                    changeMoney()
        '                    ALL_N_P.Text = Integer.Parse(ALL_N_P.Text) - 1
        '                    lineNum -= 1
        '                End If
        '            Case 1
        '                Data.Rows(lineNum - 2).Cells(4).Value += 1
        '                Data.Rows(lineNum - 2).Cells(6).Value += Data.Rows(lineNum - 2).Cells(5).Value
        '                changeMoney()
        '                ALL_N_P.Text = Integer.Parse(ALL_N_P.Text) + 1
        '        End Select
        '        cancleFlag(0) -= 1
        '    End If
        'Catch ex As Exception
        '    Login.write_errmsg(ex.Message, Me.Name, "cancleLastStep", Me)
        'End Try

        If Data.Rows.Count Then
            'Data.Rows.RemoveAt(Data.Rows.Count() - 1)
            Dim result As IAsyncResult = BeginInvoke(New Data_RemoveAt(AddressOf Data_RemoveAt_Invo), Data.Rows.Count() - 1)
            EndInvoke(result)
            lineNum -= 1
            changeMoney()
            calculatePronum()
        End If

    End Sub

    Public Delegate Sub Data_RemoveAt(line As Integer)
    Public Sub Data_RemoveAt_Invo(line As Integer)
        Data.Rows.RemoveAt(line)
    End Sub

    Private Sub F5_Click(sender As Object, e As EventArgs) Handles F5.Click
        'cancleLastStep()
        thread("cancleLastStep")
    End Sub

    Private Sub about_Click(sender As Object, e As EventArgs) Handles about.Click
        If Login.MsgboxNotice("版本：1.3" + vbCrLf + "2015年5月更新。", "版本信息", True, True, "检查更新", Me, True, False) = DialogResult.Cancel Then
            '这里加更新代码
            Login.MsgboxNotice("攻城师正在努力完善代码中，敬请期待...", "提示", False, True, Nothing, Me, True, False)
        End If
    End Sub

    Private Sub Cancellation_Click(sender As Object, e As EventArgs) Handles Cancellation.Click
        Login.key.Text = "请输入密码"
        Login.key.PasswordChar = ""
        Login.ID.Text = "请输入账号"
        Login.ID.Focus()
        Login.ActiveControl = Login.ID
        Login.Show()
        Me.Close()
    End Sub

End Class