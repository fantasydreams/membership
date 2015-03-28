Public Class balance


    Public NumId As String
    Public Max As Double = 0
    Public score As Integer = 0
    Public oldPMP As Double = 0
    Public flag As Boolean = False
    Public flag1 As Boolean = False


    Private Sub balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = Color.FromArgb(&HFFF7F7F7)
        balan.Parent = column

        ALL_M_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        VIP_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        PA_M_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        AC_PA_B.BackColor = Color.FromArgb(&HFFC5C5C5)
        PA_B_B.BackColor = Color.FromArgb(&HFFC5C5C5)

        ALL_M_ZI.BackColor = Color.FromArgb(&HFFF4F4F4)


    End Sub



End Class