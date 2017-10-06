Imports System.Drawing.Drawing2D

Public Class Circle
    Private mColor As SolidBrush
    Private mVelocity As Vector
    Private mMass As Double
    Private mID As Integer
    Private mNumber As Integer
    Private mFont As Font
    Private mAngularVelocity As Double
    Private frontAngle As Double = 90
    Private mInertia As Double
    Private mAcceleration As Vector
    Private mAngularAcceleration As Double
    Private mSize As SizeF

    Public Event PositionChanged(sender As Circle)

    Public Sub New(mass As Double, velocity As Vector, size As SizeF)
        mVelocity = New Vector(velocity)
        mMass = mass
        mSize = size

        Init()
    End Sub

    Public Sub New(color As Color, mass As Double, location As PointF, angle As Double, speed As Double, size As SizeF)
        Me.New(mass, location, angle, speed, size)
        mColor = New SolidBrush(Color)
        mFont = New Font("Small Font", 11, FontStyle.Bold, GraphicsUnit.Pixel)
    End Sub

    Public Sub New(Mass As Double, location As PointF, angle As Double, speed As Double, size As SizeF)
        Me.New(Mass, New Vector(speed, angle, location), size)
    End Sub

    Public Sub New(c As Circle)
        Me.New(c.Mass, c.Velocity, c.Size)
        mAngularVelocity = c.AngularVelocity
        Me.Number = c.Number
    End Sub

    Private Sub Init()
        mID = CInt(1000 * Rnd())
        CalcInertia()

        AddHandler mVelocity.Changed, Sub() RaiseEvent PositionChanged(Me)
    End Sub

    Private Sub CalcInertia()
        mInertia = CSng(mMass * (Me.Width ^ 2 + Me.Height ^ 2) / 12)
    End Sub

    Public Property Number() As Integer
        Get
            Return mNumber
        End Get
        Set(value As Integer)
            mNumber = value
        End Set
    End Property

    Public ReadOnly Property ID() As Integer
        Get
            Return mID
        End Get
    End Property

    Public Property Velocity() As Vector
        Get
            Return mVelocity
        End Get
        Set(value As Vector)
            If mVelocity <> value Then
                mVelocity = value
                RaiseEvent PositionChanged(Me)
            End If
        End Set
    End Property

    Public Property Mass() As Double
        Get
            Return mMass
        End Get
        Set(value As Double)
            mMass = value
            CalcInertia()
        End Set
    End Property

    Public Property X() As Double
        Get
            Return mVelocity.X1
        End Get
        Set(value As Double)
            mVelocity.X1 = value
            RaiseEvent PositionChanged(Me)
        End Set
    End Property

    Public Property Y() As Double
        Get
            Return mVelocity.Y1
        End Get
        Set(value As Double)
            mVelocity.Y1 = value
            RaiseEvent PositionChanged(Me)
        End Set
    End Property

    Public Property Location() As PointF
        Get
            Return mVelocity.Origin
        End Get
        Set(value As PointF)
            mVelocity.Origin = value
        End Set
    End Property

    Public Property Color() As Color
        Get
            Return mColor.Color
        End Get
        Set(value As Color)
            mColor = New SolidBrush(value)
        End Set
    End Property

    Public ReadOnly Property Momentum() As Vector
        Get
            Return mVelocity * mMass
        End Get
    End Property

    Public ReadOnly Property AngularVelocity() As Double
        Get
            Return mAngularVelocity
        End Get
    End Property

    Public ReadOnly Property AngularAcceleration() As Double
        Get
            Return mAngularAcceleration
        End Get
    End Property

    Public Property Inertia() As Double
        Get
            Return mInertia
        End Get
        Set(value As Double)
            mInertia = value
        End Set
    End Property

    Public Property Size() As SizeF
        Get
            Return mSize
        End Get
        Set(value As SizeF)
            mSize = Size
            CalcInertia()
        End Set
    End Property

    Public Property Width() As Double
        Get
            Return mSize.Width
        End Get
        Set(value As Double)
            Size = New SizeF(value, value)
        End Set
    End Property

    Public Property Height() As Double
        Get
            Return mSize.Height
        End Get
        Set(value As Double)
            Size = New SizeF(value, value)
        End Set
    End Property

    Public Property Radius() As Double
        Get
            Return mSize.Width / 2
        End Get
        Set(value As Double)
            Size = New SizeF(value * 2, value * 2)
        End Set
    End Property

    Public Property Diameter() As Double
        Get
            Return mSize.Width
        End Get
        Set(value As Double)
            Size = New SizeF(value, value)
        End Set
    End Property

    Public ReadOnly Property ImpactPoint() As PointF
        Get
            Return Me.ImpactVector.Destination
        End Get
    End Property

    Public ReadOnly Property ImpactVector() As Vector
        Get
            Dim pv As Vector = New Vector(mVelocity)
            pv.Magnitude = Me.Radius
            Return pv
        End Get
    End Property

    Public Sub Move(eleapsedTime As Double)
        If mVelocity.Magnitude <> 0 Then mVelocity.Move((mVelocity * eleapsedTime).Magnitude)
    End Sub

    Public Sub Paint(g As Graphics, area As Rectangle, Optional drawVector As Boolean = False)
        Static isBusy As Boolean
        If isBusy Then Exit Sub
        isBusy = True

        Dim r As New RectangleF(X - Radius, area.Height - Y - Radius, Width, Height)
        'g.FillEllipse(mColor, p.X, p.Y, Width, Height)

        Dim path As Drawing2D.GraphicsPath = New Drawing2D.GraphicsPath()
        path.AddEllipse(r)
        Dim pathBrush As Drawing2D.PathGradientBrush = New Drawing2D.PathGradientBrush(path)
        pathBrush.CenterColor = Drawing.Color.White
        pathBrush.SurroundColors = New Color() {mColor.Color}
        pathBrush.CenterPoint = New PointF(r.X + r.Width, r.Y)
        pathBrush.SetSigmaBellShape(1)
        g.FillEllipse(pathBrush, r)
        path.Dispose()
        pathBrush.Dispose()

        r.X += Radius / 2
        r.Y += Radius / 2
        g.DrawString(mNumber.ToString(), mFont, Brushes.Black, r.X + 1, r.Y + 1)
        g.DrawString(mNumber.ToString(), mFont, Brushes.White, r.X, r.Y)

        If drawVector Then
            Dim m As Vector = Momentum / 1000

            m.Paint(g, area, Drawing.Color.White)

            Dim s As String = Math.Round(m.Magnitude, 1) & " @ " & Math.Round(m.Angle, 1)
            g.DrawString(s,
                        mFont,
                        Brushes.Black,
                        mVelocity.X1 + 1, area.Height - mVelocity.Y1 + 1
                    )
            g.DrawString(s,
                        mFont,
                        Brushes.White,
                        mVelocity.X1, area.Height - mVelocity.Y1
                    )
        End If

        isBusy = False
    End Sub

    Public Shared Operator =(c1 As Circle, c2 As Circle) As Boolean
        Return c1.Number = c2.Number
    End Operator

    Public Shared Operator <>(c1 As Circle, c2 As Circle) As Boolean
        Return Not (c1 = c2)
    End Operator

    Public Shared Operator +(c1 As Circle, c2 As Circle) As Circle
        Dim size As SizeF = c1.Size
        size.Width = (size.Width + c2.Width) / 2
        size.Height = (size.Height + c2.Height) / 2

        Return New Circle(c1.Mass + c2.Mass, c1.Velocity + c2.Velocity, size)
    End Operator

    Protected Overrides Sub Finalize()
        If mFont IsNot Nothing Then mFont.Dispose()
        MyBase.Finalize()
    End Sub

    ' --------------

    Public Sub SetVelocity(newVelocity As Vector, eleapsedTime As Double)
        mAcceleration = (newVelocity - mVelocity) / eleapsedTime
        mVelocity = newVelocity
    End Sub

    Public Sub SetAngularVelocity(newAngularVelocity As Double, eleapsedTime As Double)
        mAngularAcceleration = (newAngularVelocity - mAngularVelocity) / eleapsedTime
        mAngularVelocity = newAngularVelocity
    End Sub

    Private ReadOnly Property Force() As Vector
        Get
            Return mAcceleration * mMass
        End Get
    End Property

    Private ReadOnly Property Torque() As Double
        Get
            Return mAngularAcceleration * mInertia
        End Get
    End Property
End Class