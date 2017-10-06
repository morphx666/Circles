Imports System.Runtime.CompilerServices
Imports System.Drawing

Public Class Vector
    Private Const PI As Double = Math.PI

    Private Const toRad As Double = PI / 180
    Private Const toDeg As Double = 180 / PI
    Private mMagnitude As Double
    Private mAngle As Double
    Private mOrigin As PointF
    Private angleCos As Double = 1.0
    Private angleSin As Double = 0.0

    Public Event Changed(sender As Vector)

    Public Sub New()
        Angle = 0
        mMagnitude = 1
        mOrigin = New PointF(0, 0)
    End Sub

    Public Sub New(magnitude As Double, angle As Double, origin As PointF)
        mMagnitude = Math.Abs(magnitude)
        Me.Angle = angle
        If magnitude < 0 Then Me.Angle = mAngle + 180
        mOrigin = origin
    End Sub

    Public Sub New(magnitude As Double, angle As Double, x As Double, y As Double)
        Me.New(magnitude, angle, New PointF(x, y))
    End Sub

    Public Sub New(origin As PointF, destination As PointF)
        ResetVectorFromPoints(origin.X, origin.Y, destination.X, destination.Y)
    End Sub

    Public Sub New(vector As Vector)
        Me.New(vector.Magnitude, vector.Angle, vector.Origin)
    End Sub

    Public Property Origin() As PointF
        Get
            Return mOrigin
        End Get
        Set(value As PointF)
            If mOrigin <> value Then
                mOrigin = value
                RaiseEvent Changed(Me)
            End If
        End Set
    End Property

    Public Property Destination() As PointF
        Get
            Return New PointF(X2, Y2)
        End Get
        Set(value As PointF)
            ResetVectorFromPoints(mOrigin.X, mOrigin.Y, value.X, value.Y)
        End Set
    End Property

    Public Property X1() As Double
        Get
            Return mOrigin.X
        End Get
        Set(value As Double)
            ResetVectorFromPoints(value, mOrigin.Y, X2, Y2)
        End Set
    End Property

    Public Property Y1() As Double
        Get
            Return mOrigin.Y
        End Get
        Set(value As Double)
            ResetVectorFromPoints(mOrigin.X, value, X2, Y2)
        End Set
    End Property

    Public Property X2() As Double
        Get
            Return mOrigin.X + mMagnitude * angleCos
        End Get
        Set(value As Double)
            ResetVectorFromPoints(mOrigin.X, mOrigin.Y, value, Y2)
        End Set
    End Property

    Public Property Y2() As Double
        Get
            Return mOrigin.Y + mMagnitude * angleSin
        End Get
        Set(value As Double)
            ResetVectorFromPoints(mOrigin.X, mOrigin.Y, X2, value)
        End Set
    End Property

    Public Property Magnitude() As Double
        Get
            Return mMagnitude
        End Get
        Set(value As Double)
            If value <> mMagnitude Then
                If mMagnitude = 0.00009999999 Then mMagnitude = 0

                mMagnitude = Math.Abs(value)
                If value < 0 Then Angle = mAngle + 180
            End If
        End Set
    End Property

    Public Property Angle() As Double
        Get
            Return mAngle
        End Get
        Set(value As Double)
            If value <> mAngle Then
                If value < 0 Then value += 360
                mAngle = value Mod 360

                If mAngle = 0 Then
                    angleCos = 1.0
                    angleSin = 0.0
                Else
                    Dim radAngle As Double = mAngle * toRad
                    angleCos = Math.Cos(radAngle)
                    angleSin = Math.Sin(radAngle)
                End If
                RaiseEvent Changed(Me)
            End If
        End Set
    End Property

    Public ReadOnly Property Slope() As Double
        Get
            Return (Y2 - Y1) / (X2 - X1)
        End Get
    End Property

    Public Sub Move(offset As Double)
        Dim ov As Vector = New Vector(offset, mAngle, mOrigin)
        Origin = ov.Destination
    End Sub

    Private Sub ResetVectorFromPoints(px1 As Double, py1 As Double, px2 As Double, py2 As Double)
        Dim v As Vector = Vector.VectorFromPoints(px1, py1, px2, py2)

        mMagnitude = v.Magnitude
        mOrigin = v.Origin
        Angle = v.Angle
    End Sub

    Private Shared Function VectorFromPoints(px1 As Double, py1 As Double, px2 As Double, py2 As Double) As Vector
        Dim v As Vector = New Vector()
        Dim dx As Double = px2 - px1
        Dim dy As Double = py2 - py1

        v.Angle = Vector.fAtan(dx, dy)
        v.Magnitude = Vector.Distance(dx, dy)
        v.Origin = New PointF(px1, py1)

        Return v
    End Function

    Public Shared Function Normalize(p1 As PointF, p2 As PointF) As Vector
        Dim v As Vector = Vector.VectorFromPoints(p1.X, p1.Y, p2.X, p2.Y)
        v.Magnitude = 1
        Return v
    End Function

    Public Shared Function fAtan(dx As Double, dy As Double) As Double
        Dim a As Double

        If dy = 0 Then
            If dx > 0 Then
                a = 0
            Else
                a = 180
            End If
        Else
            a = Math.Atan(dy / dx) * toDeg
            Select Case a
                Case Is > 0
                    If dx < 0 AndAlso dy < 0 Then a += 180
                Case 0
                    If dx < 0 Then a = 180
                Case Is < 0
                    If dy > 0 Then
                        If dx > 0 Then
                            a = Math.Abs(a)
                        Else
                            a += 180
                        End If
                    Else
                        a += 360
                    End If
            End Select
        End If

        Return a
    End Function

    Public Sub Paint(g As Graphics, area As Rectangle, c As Color, Optional s As Integer = 1)
        If s = 0 OrElse Me.Magnitude = 0 Then Exit Sub

        Dim yo As Double = area.Height
        Dim v As Vector = New Vector(Me)

        v.Magnitude *= s
        Using p As New Pen(c, 2)
            g.DrawLine(p, v.X1, yo - v.Y1, v.X2, yo - v.Y2)

            v.Origin = v.Destination
            v.Angle += 170
            v.Magnitude /= 3
            g.DrawLine(p, v.X1, yo - v.Y1, v.X2, yo - v.Y2)

            v.Angle += 20
            g.DrawLine(p, v.X1, yo - v.Y1, v.X2, yo - v.Y2)
        End Using
    End Sub

    Public Sub Transform(angle As Double, p As PointF)
        Dim dx As Double = X1 - p.X
        Dim dy As Double = Y1 - p.Y
        Dim d As Double = Vector.Distance(dx, dy)

        Dim a As Double = (Vector.fAtan(dx, dy) + angle) * toRad

        Dim xp1 As Double = p.X + d * Math.Cos(a)
        Dim yp1 As Double = p.Y + d * Math.Sin(a)

        dx = X2 - p.X
        dy = Y2 - p.Y
        d = Vector.Distance(dx, dy)
        a = (Vector.fAtan(dx, dy) + angle) * toRad

        ResetVectorFromPoints(xp1, yp1, p.X + d * Math.Cos(a), p.Y + d * Math.Sin(a))
    End Sub

    Public Shared Operator =(v1 As Vector, v2 As Vector) As Boolean
        Return v1.Angle = v2.Angle AndAlso v1.Magnitude = v2.Magnitude
    End Operator

    Public Shared Operator <>(v1 As Vector, v2 As Vector) As Boolean
        Return Not (v1 = v2)
    End Operator

    Public Shared Operator +(v1 As Vector, v2 As Vector) As Vector
        Dim v3 As Vector = New Vector(v2)
        v3.Origin = v1.Destination
        Return New Vector(v1.Origin, v3.Destination)
    End Operator

    Public Shared Operator -(v1 As Vector, v2 As Vector) As Vector
        Dim v3 As Vector = New Vector(v2)
        v3.Origin = v1.Origin
        Return New Vector(v3.Destination, v1.Destination)
    End Operator

    Public Shared Operator *(v1 As Vector, s As Double) As Vector
        Return New Vector(v1.Magnitude * s, v1.Angle, v1.Origin)
    End Operator

    Public Shared Operator *(s As Double, v1 As Vector) As Vector
        Return v1 * s
    End Operator

    Public Shared Operator /(v1 As Vector, s As Double) As Vector
        Return v1 * (1 / s)
    End Operator

    Public Shared Operator ^(v1 As Vector, power As Double) As Double
        If power = 2 Then Return Vector.Dot(v1, v1)
        Return v1.Magnitude ^ power
    End Operator

    Public Shared Function Dot(ByRef v1 As Vector, ByRef v2 As Vector) As Double
        Dim a As Double = Math.Abs(v1.Angle - v2.Angle)
        If a > 180 Then a = 360 - a

        Return v1.Magnitude * v2.Magnitude * Math.Cos(a * toRad)
    End Function

    Public Shared Function Cross(v1 As Vector, v2 As Vector) As Double
        Dim Rx As Double = v1.X2 - v1.X1
        Dim Ry As Double = v1.Y2 - v1.Y1

        Dim Tx As Double = v2.X2 - v2.X1
        Dim Ty As Double = v2.Y2 - v2.Y1

        Return Rx * Ty - Ry * Tx
    End Function

    Public Shared Function Cross(v1 As Vector, s As Double) As Vector
        'Dim v As Vector = New Vector(v1.Origin, New PointF(v1.x1 + -s * v1.y2, v1.y1 + s * v1.x2))
        Dim v As Vector = New Vector(v1.Origin, New PointF(v1.X1 + -s * (v1.Y2 - v1.Y1), v1.Y1 + s * (v1.X2 - v1.X1)))
        'Dim v As Vector = New Vector(v1.Origin, New PointF(-s * v1.y1, s * v1.x1))
        Return v
    End Function

    Public Shared Function Cross(s As Double, v1 As Vector) As Vector
        Return Vector.Cross(v1, s)
    End Function

    Public Shared Function Distance(x1 As Double, y1 As Double, x2 As Double, y2 As Double) As Double
        Return Distance(x2 - x1, y2 - y1)
    End Function

    Public Shared Function Distance(dx As Double, dy As Double) As Double
        Return Math.Sqrt(dx ^ 2 + dy ^ 2)
    End Function

    Public Shared Function Distance(p1 As PointF, p2 As PointF) As Double
        Return Distance(p1.X, p1.Y, p2.X, p2.Y)
    End Function

    Public Overrides Function ToString() As String
        Return String.Format("Magnitude: {0}" & vbCrLf &
                                "Angle: {1}" & vbCrLf &
                                "({2}, {3})-({4}, {5})" & vbCrLf &
                                "y = {6}x + {7}",
                                mMagnitude,
                                mAngle,
                                X1, Y1, X2, Y2,
                                Slope, X1
                            )
    End Function
End Class

Module ModuleExtensions
    <Extension()>
    Public Sub DrawLine(g As Graphics, p As Pen, x1 As Double, y1 As Double, x2 As Double, y2 As Double)
        g.DrawLine(p, Convert.ToSingle(x1), Convert.ToSingle(y1),
                   Convert.ToSingle(x2), Convert.ToSingle(y2))
    End Sub

    <Extension()>
    Public Sub DrawEllipse(g As Graphics, p As Pen, x As Double, y As Double, width As Double, height As Double)
        g.DrawEllipse(p, Convert.ToSingle(x), Convert.ToSingle(y),
                   Convert.ToSingle(width), Convert.ToSingle(height))
    End Sub

    <Extension()>
    Public Sub FillEllipse(g As Graphics, b As Brush, x As Double, y As Double, width As Double, height As Double)
        g.FillEllipse(b, Convert.ToSingle(x), Convert.ToSingle(y),
                   Convert.ToSingle(width), Convert.ToSingle(height))
    End Sub
End Module