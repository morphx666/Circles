Imports System.Threading

Public Class frmMain
    Private circles As CirclesCollection = New CirclesCollection()

    Private speedsHistory As Dictionary(Of Integer, List(Of Double)) = New Dictionary(Of Integer, List(Of Double))
    Private maxSpeed As Dictionary(Of Integer, Double) = New Dictionary(Of Integer, Double)

    Private selCircle As Circle
    Private dragVector As Vector
    Private isDragging As Boolean
    Private colVector As Vector
    Private colVectorAlpha As Integer = 0

    Private refreshThread As Thread
    Private cancelThread As Boolean

    Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        cancelThread = True
        Circles.Dispose()
    End Sub

    Private Sub frmMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.DoubleBuffered = True
        Me.BackColor = Color.Green

        Reset()

        refreshThread = New Thread(AddressOf DoRepaint)
        refreshThread.Start()
    End Sub

    Private Sub Reset()
        Circles.PauseSimulation()

        Circles.Clear()
        Circles.ClientArea = Me.DisplayRectangle
        speedsHistory.Clear()
        maxSpeed.Clear()

        gbOptions.Visible = True
    End Sub

    Private Sub SetupAsNewtonsCradle()
        Dim mass As Double = 300
        Dim size As SizeF = New SizeF(30, 30)
        Dim n As Integer = 5
        Dim p As PointF = New PointF(CSng(Width / 2 - 2 * size.Width - size.Width / 2), Height \ 2)
        For i As Integer = 1 To 5
            Circles.AddWithNumber(New Circle(Color.FromArgb(255, CInt(127 + 128 * Rnd()), CInt(127 + 128 * Rnd()), CInt(127 + 128 * Rnd())), mass, p, 0, 0, size), i)

            p.X += (size.Width + 1)
        Next

        p.X += size.Width * 2
        Dim cueBall As Circle = New Circle(Color.White, mass, p, 0, 0, size)
        Circles.Add(cueBall)

        chkSimulatePockets.Checked = False
        chkSimulatePockets.Enabled = False
    End Sub

    Private Sub SetupAsPoolSimulation()
        Dim mass As Double = 300
        Dim size As SizeF = New SizeF(30, 30)
        Dim n As Integer = 5
        Dim c As Integer = 1
        Dim p As PointF = New PointF(CSng(Width / 2 - 2 * size.Width - size.Width / 2), Height - 200)
        For i As Integer = 1 To 15
            Circles.AddWithNumber(New Circle(Color.FromArgb(255, CInt(127 + 128 * Rnd()), CInt(127 + 128 * Rnd()), CInt(127 + 128 * Rnd())), mass, p, 0, 0, size), i)

            p.X += (size.Width + 1)

            If c = n Then
                n -= 1
                c = 1
                p.X = CSng(Width / 2 - (n - 1) * size.Width - (size.Width / 2) * (2 - n))

                Dim v As Vector = New Vector(Circles(Circles.Count - 1).Velocity)
                v.Angle = 300 - 60
                v.Magnitude = size.Width

                p.Y = CSng(v.Y2 - 1)
            Else
                c += 1
            End If
        Next

        Dim cueBall As Circle = New Circle(Color.White, mass, New PointF(CSng(Width / 2), size.Width), 0, 0, size)
        Circles.Add(cueBall)

        chkSimulatePockets.Enabled = True
    End Sub

    Private Sub CheckIfCircleIsInPocket(circle As Circle)
        If chkSimulatePockets.Checked Then
            Dim a As Double = circle.Diameter
            If circle.X <= a AndAlso Circles.Bounds.Height - circle.Y <= a Then
                Circles.QueRemove(circle)
            ElseIf circle.X >= Circles.Bounds.Width - a AndAlso Circles.Bounds.Height - circle.Y <= a Then
                Circles.QueRemove(circle)
            ElseIf circle.X <= a AndAlso circle.Y <= a Then
                Circles.QueRemove(circle)
            ElseIf circle.X >= Circles.Bounds.Width - a AndAlso circle.Y <= a Then
                Circles.QueRemove(circle)
            End If
        End If
    End Sub

    Private Sub CollisionPoint(n As Vector)
        colVector = n
        colVectorAlpha = 255
        Me.Invalidate()
    End Sub

    Private Sub DoRepaint()
        Do Until cancelThread
            Thread.Sleep(15)
            Me.Invalidate()
        Loop
    End Sub

    Private Sub frmMain_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        selCircle = Circles.GetCircleAt(e.X, Me.ClientSize.Height - e.Y)
        If selCircle IsNot Nothing Then
            selCircle.Velocity.Magnitude = 0
            selCircle.SetAngularVelocity(selCircle.AngularVelocity, 1)
            dragVector = New Vector(0, 0, selCircle.X, selCircle.Y)
            Me.Invalidate()
            isDragging = True
        End If
    End Sub

    Private Sub frmMain_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If isDragging Then
            dragVector.Destination = New PointF(e.X, Me.DisplayRectangle.Height - e.Y)
            'Me.Invalidate()
        End If
    End Sub

    Private Sub frmMain_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If isDragging Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                selCircle.Velocity = dragVector * 10
            Else
                selCircle.X = e.X
                selCircle.Y = Me.ClientSize.Height - e.Y
                selCircle.Velocity.Magnitude = 0
            End If
            isDragging = False
        End If
    End Sub

    Private Sub frmMain_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim g As Graphics = e.Graphics

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Draw Speed Graph for the balls
        If chkSpeedGraphs.Checked AndAlso speedsHistory.Count > 1 Then
            Dim p As PointF
            Dim y As Integer
            Dim h As Integer = Me.DisplayRectangle.Height \ speedsHistory.Count

            Using lFont As New Font(Me.Font, FontStyle.Bold)
                For Each circleSpeeds As KeyValuePair(Of Integer, List(Of Double)) In speedsHistory
                    If speedsHistory.Item(0).Count >= Me.DisplayRectangle.Width Then circleSpeeds.Value.RemoveAt(0)

                    maxSpeed.Item(circleSpeeds.Key) = Math.Max(circleSpeeds.Value.Max(), maxSpeed.Item(circleSpeeds.Key))
                    Using gPen As New Pen(circles.Item(circleSpeeds.Key).Color)
                        p = New Point(0, 0)
                        y = circleSpeeds.Key * h + h
                        For Each speed As Double In circleSpeeds.Value
                            p = New PointF(p.X, CSng(y - (speed / maxSpeed.Item(circleSpeeds.Key)) * h))
                            g.DrawLine(gPen, p.X, p.Y, p.X, y)
                            p.X += 1
                        Next
                        g.DrawString(circleSpeeds.Key.ToString(), lFont, Brushes.Black, 5, y - h \ 2)
                    End Using
                Next
            End Using

            For Each circleSpeeds As KeyValuePair(Of Integer, List(Of Double)) In speedsHistory
                p = New Point(0, 0)
                y = circleSpeeds.Key * h + h
                g.DrawLine(Pens.DarkGray, 0, y, Me.DisplayRectangle.Width, y)
            Next
        End If

        Dim rh As Double = Me.DisplayRectangle.Height

        For Each c As Circle In circles
            Try
                c.Paint(g, Me.DisplayRectangle, False)
            Catch ex As Exception
                ' Ignore when trying to paing outside a "paintable" region
            End Try
            If isDragging AndAlso c = selCircle Then
                g.DrawEllipse(Pens.Red, c.X - c.Radius, rh - c.Y - c.Radius, c.Width, c.Height)
                dragVector.Paint(g, Me.DisplayRectangle, Color.White)
            End If

            If Not speedsHistory.ContainsKey(c.Number) Then
                speedsHistory.Add(c.Number, New List(Of Double))
                maxSpeed.Add(c.Number, 0)
            End If
            speedsHistory(c.Number).Add(c.Velocity.Magnitude)
        Next

        If colVectorAlpha >= 0 AndAlso colVector IsNot Nothing Then
            colVector.Paint(g, Me.DisplayRectangle, Color.FromArgb(colVectorAlpha, Color.Black), 20)
            colVectorAlpha -= 8
        End If
    End Sub

    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        Reset()
    End Sub

    Private Sub frmMain_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        If Circles IsNot Nothing Then Circles.Bounds = Me.DisplayRectangle
    End Sub

    Private Sub btnRun_Click(sender As System.Object, e As System.EventArgs) Handles btnRun.Click
        gbOptions.Visible = False

        If rbPoolSimulation.Checked Then SetupAsPoolSimulation()
        If rbNewtonsCradle.Checked Then SetupAsNewtonsCradle()

        Circles.RunSimulation()
        AddHandler Circles.CollisionPoint, AddressOf CollisionPoint
        AddHandler Circles.CirclePositionChanged, AddressOf CheckIfCircleIsInPocket
    End Sub
End Class
