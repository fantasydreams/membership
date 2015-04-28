﻿Imports MySql.Data.MySqlClient
Imports System.IO



Public Class fristrun

    Public conn As New MySqlConnection
    Dim datatable As New DataTable
    Dim flag As Integer = -1  '标记商店的下标
    Dim connectflag As Boolean = False
    Private Sub invo()
        BeginInvoke(New eventhandler(AddressOf connect), Nothing)
    End Sub

    Public Sub connect()
        Dim connStr As String
        If Not conn Is Nothing Then conn.Close()
        'connStr = String.Format("server={0};user id={1}; password={2}; database=member; pooling=false;charset=utf8", _
        '"112.74.105.67", "ming", "18883285787")
        Dim hostname As String = "www.myvip6.com"
        Dim IPs() As System.Net.IPAddress = System.Net.Dns.GetHostAddresses(hostname) '根据域名获取ip地址
        For I = 0 To UBound(IPs)
            connStr = String.Format("server={0};user id={1}; password={2}; database=member; pooling=false;charset=utf8", _
        IPs(I).ToString, "ming", "18883285787")
            Try
                conn = New MySqlConnection(connStr)
                conn.Open()
                If conn.State = ConnectionState.Open Then
                    connectflag = True
                    BeginInvoke(New eventhandler(AddressOf getshopNameandId), Nothing)
                End If
                '    MsgBox("开")
                'Else
                '    MsgBox("关")
                'End If
            Catch ex As MySqlException
                MessageBox.Show("Error connecting to the server: " + ex.Message)
                'runThread.Abort()
            End Try
            If connectflag Then
                Exit Sub
            End If
        Next I
        
    End Sub

    Private Sub fristrun_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.BackColor = Color.White
        Yes_Button.Parent = Yes_pic
        invo() '开始异步线程
    End Sub
    '获取商家名称和ID
    Private Sub getshopNameandId()
        Try
            datatable.Reset()
            Dim da As MySqlDataAdapter
            Dim cb As MySqlCommandBuilder
            da = New MySqlDataAdapter("select id,name from shop;", conn)
            cb = New MySqlCommandBuilder(da)
            da.Fill(datatable)
            If (datatable.Rows.Count()) Then
                For i = 0 To datatable.Rows.Count() - 1
                    shop_list.Items.Add(datatable.Rows.Item(i).Item(1).ToString())
                Next
                shop_list.Text = datatable.Rows.Item(0).Item(1).ToString()
            End If
        Catch

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
        If e.KeyChar = Chr(Keys.Enter) Then
            For i = 0 To datatable.Rows.Count() - 1
                If shop_id_in.Text = datatable.Rows.Item(i).Item(0) Then
                    shop_list.Text = datatable.Rows.Item(i).Item(1)
                    Exit Sub
                End If
            Next
            'Dim msgform As New MSG
            'msgform.msgP.Text = "没有以编号为：" & shop_id_in.Text & "的超市！请检查编号是否准确。"
            'msgform.head.Text = "提示"
            'msgform.Show()
            Login.MsgboxNotice("没有以编号为：" & shop_id_in.Text & "的超市！请检查编号是否准确。", "提示", False, True, Nothing, Me)
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
        For i = 0 To datatable.Rows.Count - 1
            If shop_list.Text = datatable.Rows.Item(i).Item(1) Then
                shop_id_in.Text = datatable.Rows.Item(i).Item(0).ToString()
                Exit Sub
            End If
        Next
    End Sub


    Private Sub Yes_Button_Click(sender As Object, e As EventArgs) Handles Yes_Button.Click
        'Dim formmsg As New MSG
        'formmsg.msgP.Text = "您的店铺ID为：" & shop_id_in.Text + vbCrLf + "您的店铺名称为：" & shop_list.Text
        'formmsg.head.Text = "消息"
        'formmsg.yes.Visible = True
        'formmsg.yes_button.Visible = True
        'formmsg.no.Visible = True
        'formmsg.no_button.Visible = True
        If Login.MsgboxNotice("您的店铺ID为：" & shop_id_in.Text + vbCrLf + "您的店铺名称为：" & shop_list.Text, "消息", True, True, "重新选择", Me) = Windows.Forms.DialogResult.OK Then
            Dim fsr As New StreamWriter(".\config\data.ini")  '向文件中写入配置信息
            fsr.WriteLine(shop_id_in.Text)
            fsr.WriteLine(shop_list.Text)
            fsr.Close()
            conn.Close() '关闭sql连接
            Login.shopID = Long.Parse(shop_id_in.Text)
            'MsgBox(Login.shopID)
            Me.Close() '关闭本窗口
        End If
    End Sub
End Class