Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Threading



Public Class fristrun

    Private conn As New MySqlConnection
    Dim datatable As New DataTable
    Dim flag As Integer = -1  '标记商店的下标
    Dim connectflag As Boolean = False
    Dim shop As Boolean = False
    Delegate Sub MyDelegate(datatable As DataTable)
    Delegate Sub vdeletegate()
    Dim connectting_run As Boolean = False  '阻止在数据库连接区间进行刷新而导致的错误

    Private Sub DelegateMethod(datatable As DataTable)
        If (datatable.Rows.Count()) Then
            For i = 0 To datatable.Rows.Count() - 1
                shop_list.Items.Add(datatable.Rows.Item(i).Item(1).ToString())
            Next
            shop_list.Text = datatable.Rows.Item(0).Item(1).ToString()
            shop = True
        End If
    End Sub 'DelegateMethod
    Delegate Function idtoshop_name()
    Private Function shopNamemethod()
        For i = 0 To datatable.Rows.Count() - 1
            If shop_id_in.Text = datatable.Rows.Item(i).Item(0) Then
                shop_list.Text = datatable.Rows.Item(i).Item(1)
                Return True
            End If
        Next
        Return False
    End Function 'shopName method

    Private Sub Load_Data()
        Dim fsr As New StreamWriter(".\config\data.ini")  '向文件中写入配置信息
        fsr.WriteLine(shop_id_in.Text)
        fsr.WriteLine(shop_list.Text)
        fsr.Close()
        If Login.CheckApplicationIsRun(Application.StartupPath + "./membersync.exe") Then '如果同步程序在运行，则杀死进程
            Dim myprocess() As Process
            Dim p As New System.Diagnostics.Process
            Dim inst As Process
            Try
                myprocess = System.Diagnostics.Process.GetProcessesByName("membersync")
                For Each inst In myprocess
                    p = System.Diagnostics.Process.GetProcessById(inst.Id)
                    p.Kill()
                Next
            Catch ex As Exception

            End Try
        End If
        Dim pro As Process = Process.Start(Application.StartupPath + "./sync.exe", shop_id_in.Text + " 1") '启动外部同步程序
        Dim str As String = "select frist_result from frist_run where id = 1"
        Dim table As New DataTable
        If Login.sqliteconn.State <> ConnectionState.Open Then
            Login.connect()
        End If
        While True   '等连接成功后向下执行
            If Login.sqliteconn.State <> ConnectionState.Open Then
                Exit While
            End If
            Thread.Sleep(100)  '线程休眠100ms
        End While

        Dim cmd As New SQLite.SQLiteCommand
        cmd.Connection = Login.sqliteconn
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "update frist_run set frist_result = 0 where id = 1"
        cmd.ExecuteNonQuery()

        While True  '同步完成后向下执行
            Dim da As New SQLite.SQLiteDataAdapter(str, Login.sqliteconn)
            table.Reset()
            da.Fill(table)
            If (table.Rows.Item(0).Item(0).ToString <> "0") Then
                Exit While
            End If
            Thread.Sleep(100)  '线程休眠100ms
        End While
        conn.Close() '关闭sql连接
    End Sub

    Private Sub writeshopmsg()
        If Login.MsgboxNotice("您的店铺ID为：" & shop_id_in.Text + vbCrLf + "您的店铺名称为：" & shop_list.Text, "消息", True, True, "重新选择", Me, True, True) = Windows.Forms.DialogResult.OK Then
            Dim tip As New load_tip
            BeginInvoke(New Login.windows_show(AddressOf Login.win_show), tip, Me)

            
            Login.shopID = Long.Parse(shop_id_in.Text)
            BeginInvoke(New Login.windows_hide(AddressOf Login.win_close), tip)
            Me.Close() '关闭本窗口
        End If
    End Sub

    Private Sub invo()
        Dim connectThread As New Thread(AddressOf connect)
        connectThread.Start() '开始连接线程
    End Sub

    Public Sub connect()
        connectting_run = True
        Dim connStr As String
        If Not conn Is Nothing Then conn.Close()
        'connStr = String.Format("server={0};user id={1}; password={2}; database=member; pooling=false;charset=utf8", _
        '"112.74.105.67", "ming", "18883285787")
        Dim hostname As String = "www.myvip6.com"
        Dim IPs() As System.Net.IPAddress = Nothing
        'If Not My.Computer.Network.IsAvailable Then
        '    BeginInvoke(New EventHandler(AddressOf msgbox_net))
        '    Exit Sub
        'End If
        Try
            IPs = System.Net.Dns.GetHostAddresses(hostname) '根据域名获取ip地址
            For I = 0 To UBound(IPs)
                connStr = String.Format("server={0};user id={1}; password={2}; database=member; pooling=false;charset=utf8", _
            IPs(I).ToString, "ming", "18883285787")

                conn = New MySqlConnection(connStr)
                conn.Open()
                If conn.State = ConnectionState.Open Then
                    connectflag = True
                    Dim tip As New load_tip
                    BeginInvoke(New Login.windows_show(AddressOf Login.win_show), tip, Nothing)
                    getshopNameandId(tip)
                Else
                    'Login.MsgboxNotice("无法从服务器获取信息，请检查网络连接！", "提示", False, False, Nothing, Me, True, False)
                    BeginInvoke(New Login.msgbox_de(AddressOf Login.msgbox_invo), "无法从服务器获取信息，请检查网络连接！", "提示", False, False, Nothing, Me, True, False)
                End If
                '    MsgBox("开")
                'Else
                '    MsgBox("关")
                'End If
                If connectflag Then
                    connectting_run = False
                    Exit Sub
                End If
            Next I
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        Catch ex As Exception
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            connectting_run = False
            BeginInvoke(New Login.write_err_msg(AddressOf Login.Write_Err_Msg_Invo), "获取信息失败，请检查网络是否畅通...", Me.Name, "connect", Me)
        End Try
        connectting_run = False
    End Sub

    Private Sub fristrun_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.BackColor = Color.White
        Yes_Button.Parent = Yes_pic
        'Control.CheckForIllegalCrossThreadCalls = False
        invo() '开始异步线程
    End Sub
    '获取商家名称和ID
    Private Sub getshopNameandId(tips As Form)
        Try
            datatable.Reset()
            Dim da As MySqlDataAdapter
            Dim cb As MySqlCommandBuilder
            da = New MySqlDataAdapter("select id,name from shop;", conn)
            cb = New MySqlCommandBuilder(da)
            da.Fill(datatable)
            Dim result As IAsyncResult = BeginInvoke(New MyDelegate(AddressOf DelegateMethod), datatable)
            EndInvoke(result)
            BeginInvoke(New Login.windows_close(AddressOf Login.win_close), tips)
        Catch ex As Exception
            'Login.write_errmsg(ex.Message.ToString, Me.Name, "getshopNameandId", Me)
            BeginInvoke(New Login.write_err_msg(AddressOf Login.Write_Err_Msg_Invo), ex.Message, Me.Name, "getshopNameandId", Me)
        End Try

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
        If e.KeyChar = Chr(Keys.Enter) And connectflag Then
            Dim result As IAsyncResult = BeginInvoke(New idtoshop_name(AddressOf shopNamemethod))
            If Not EndInvoke(result) Then
                Login.MsgboxNotice("没有以编号为：" & shop_id_in.Text & "的超市！" + vbCrLf + "请检查编号是否准确。", "提示", False, False, Nothing, Me, True, False)
            End If
        End If
        If connectflag = False Then
            Login.MsgboxNotice("无法获取信息，请检查网络连接！", "提示", False, False, Nothing, Me, True, False)
        End If
    End Sub

    '不处理选择框中的任何输入
    Private Sub shop_list_KeyPress(sender As Object, e As KeyPressEventArgs) Handles shop_list.KeyPress
        e.Handled = True
    End Sub

    Private Sub close_window_Click(sender As Object, e As EventArgs) Handles close_window.Click
        If Not Login.shopreset Then
            End '关闭窗体
        End If
        Me.Close()
    End Sub

    Private Sub shop_list_SelectedIndexChanged(sender As Object, e As EventArgs) Handles shop_list.SelectedIndexChanged
        BeginInvoke(New EventHandler(AddressOf shop_Select_index_c_invo))
    End Sub

    Private Sub shop_Select_index_c_invo()
        For i = 0 To datatable.Rows.Count - 1
            If shop_list.Text = datatable.Rows.Item(i).Item(1) Then
                shop_id_in.Text = datatable.Rows.Item(i).Item(0).ToString()
                Exit Sub
            End If
        Next
    End Sub

    Private Sub Yes_Button_Click(sender As Object, e As EventArgs) Handles Yes_Button.Click
        If shop Then
            BeginInvoke(New EventHandler(AddressOf writeshopmsg))
        End If
    End Sub

    Private Sub me_key_down(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            close_window_Click(Me, Nothing)
        End If
        If e.KeyCode = Keys.F5 And Not connectting_run Then
            shop_list.Items.Clear()
            invo()
        End If
    End Sub
End Class