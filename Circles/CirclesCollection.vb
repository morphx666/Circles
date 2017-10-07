' Most of this code was implemented from the excellent information found at http://www.myphysicslab.com/collision.html

Imports System.Threading

Public Class CirclesCollection
    Implements IList(Of Circle), IDisposable

    Private simulationThread As Thread

    Private mIsSimulating As Boolean
    Private mCircles As IList(Of Circle) = New List(Of Circle)
    Private mBounds As Rectangle
    Private mSuspendSimulation As Boolean

    Private queRemoveCircles As New List(Of Circle)

    ' http://billiards.colostate.edu/threads/physics.html
    Private Const timeStep As Double = 0.0008
    Private Const friction As Double = 0.06
    Private Const e As Double = 0.8   ' Coefficient of restitution

    Public Event CollisionPoint(vector As Vector)
    Public Event CirclePositionChanged(sender As Circle)

    Private Structure CollisionResult
        Dim Circle1FinalVelocity As Vector
        Dim Circle2FinalVelocity As Vector
        Dim Circle1FinalAngularVelocity As Double
        Dim Circle2FinalAngularVelocity As Double

        Public Sub New(c1fv As Vector, c2fv As Vector, c1fav As Double, c2fav As Double)
            Circle1FinalVelocity = c1fv
            Circle2FinalVelocity = c2fv

            Circle1FinalAngularVelocity = c1fav
            Circle2FinalAngularVelocity = c2fav
        End Sub
    End Structure

    Public Sub New(c() As Circle)
        For i As Integer = 0 To c.Length - 1
            Me.Add(c(i))
        Next
    End Sub

    Public Sub New()
        simulationThread = New Thread(AddressOf SimulationSub)
        simulationThread.Name = "CirclesAnimation"
    End Sub

    Public Property Bounds() As Rectangle
        Get
            Return mBounds
        End Get
        Set(value As Rectangle)
            mBounds = value
        End Set
    End Property

    Public Sub PauseSimulation()
        mSuspendSimulation = True
    End Sub

    Public Sub ResumeSimulation()
        mSuspendSimulation = False
    End Sub

    Public Property ClientArea As Rectangle
        Get
            Return mBounds
        End Get
        Set(value As Rectangle)
            mBounds = value
        End Set
    End Property

    Public Sub RunSimulation()
        If simulationThread.ThreadState = ThreadState.Unstarted Then simulationThread.Start()
        If mSuspendSimulation Then Me.ResumeSimulation()
    End Sub

    Public ReadOnly Property IsSimulating As Boolean
        Get
            Return mIsSimulating
        End Get
    End Property

    Private Sub SimulationSub()
        If mIsSimulating Or mSuspendSimulation Then Exit Sub
        mIsSimulating = True

        Dim tStart As Date
        Dim execTime As Integer


        Do While simulationThread.ThreadState = ThreadState.Running AndAlso Me.disposedValue = False
            Try
                If execTime <= (timeStep * 1000) Then
                    Thread.Sleep(CInt(timeStep * 1000 - execTime))
                End If

                tStart = Now
                For Each c1 As Circle In mCircles
                    If c1.Velocity.Magnitude = 0 Then Continue For

                    DoWallCollision(c1, timeStep)
                    RunCollisionAnalysis(c1, timeStep)

                    c1.Move(timeStep)
                    c1.Velocity.Magnitude -= CSng(Math.Sqrt(c1.Velocity.Magnitude) * friction)
                    RaiseEvent CirclePositionChanged(c1)

                    If c1.AngularVelocity > 0 Then
                        c1.SetAngularVelocity(c1.AngularVelocity * 2 - friction, timeStep)
                        If c1.AngularVelocity < 0 Then c1.SetAngularVelocity(c1.AngularVelocity, timeStep)
                    ElseIf c1.AngularVelocity < 0 Then
                        c1.SetAngularVelocity(c1.AngularVelocity * 2 + friction, timeStep)
                        If c1.AngularVelocity > 0 Then c1.SetAngularVelocity(c1.AngularVelocity, timeStep)
                    End If
                Next

                If queRemoveCircles.Count > 0 Then
                    For Each c1 As Circle In queRemoveCircles
                        mCircles.Remove(c1)
                    Next
                    queRemoveCircles.Clear()
                End If

                execTime = CInt(Now.Subtract(tStart).Duration.TotalMilliseconds)
                'If execTime > 0 Then Stop
            Catch
            End Try
        Loop

        mIsSimulating = False
    End Sub

    Private Function RunCollisionAnalysis(c1 As Circle, eleapsedTime As Double) As Boolean
        Dim collisionResult As CollisionResult
        Dim result As Boolean = False
        Dim collisionAmmount As Double

        ' Perform the analysis with a circle that is one step ahead of the actual circle.
        ' This way we can determine if the circles would collide before they actually do.
        Dim cTest As Circle = New Circle(c1)
        cTest.Move(eleapsedTime)

        For Each c2 As Circle In mCircles
            If cTest <> c2 Then
                collisionAmmount = TestForCollision(cTest, c2)
                If collisionAmmount < 0 Then
                    result = True

                    Dim cc As CirclesCollection = GetCSum(c2, New CirclesCollection(New Circle() {c1}))
                    Dim cx As Circle = cc.Sum()
                    collisionResult = DoCollision(c1, cx)

                    c1.SetVelocity(collisionResult.Circle1FinalVelocity, eleapsedTime)
                    c1.SetAngularVelocity(collisionResult.Circle1FinalAngularVelocity, eleapsedTime)

                    For Each c As Circle In cc
                        ' Newton's law of motion: conservation of linear momentum (http://en.wikipedia.org/wiki/Conservation_of_momentum#Conservation_of_linear_momentum)
                        c.SetVelocity(New Vector(collisionResult.Circle2FinalVelocity.Magnitude * c.Mass / cx.Mass, collisionResult.Circle2FinalVelocity.Angle, c.Location), eleapsedTime)
                        c.SetAngularVelocity(collisionResult.Circle2FinalAngularVelocity * c.Mass / cx.Mass, eleapsedTime)
                        c.Move(eleapsedTime)
                    Next
                End If
            End If

            If simulationThread.ThreadState <> ThreadState.Running Then Exit For
        Next

        Return result
    End Function

    Private Function GetCSum(c1 As Circle, ignoreList As CirclesCollection) As CirclesCollection
        Dim cc As CirclesCollection = New CirclesCollection(New Circle() {c1})
        Dim il As CirclesCollection = New CirclesCollection

        For Each c As Circle In ignoreList
            il.Add(c)
        Next

        For Each c As Circle In mCircles
            If Not ignoreList.Contains(c) And c <> c1 Then
                If TestForCollision(c1, c) <> 0 Then
                    il.Add(c)
                    il.Add(c1)
                    cc.AddCollection(GetCSum(c, il))
                End If
            End If
        Next

        Return cc
    End Function

    Private Function DoCollision(ca As Circle, cb As Circle) As CollisionResult
        ' Normal to the point of impact (impulse)
        '   n.Origin = Point of impact
        Dim n As Vector = New Vector(cb.Location, ca.Location)
        n.Magnitude = 1
        n.Move(cb.Diameter)
        'RaiseEvent CollisionPoint(n)

        ' Distance vector between the center of mass and the point of impact
        Dim rap As Vector = New Vector(ca.Location, n.Origin)
        Dim rbp As Vector = New Vector(cb.Location, n.Origin)

        ' Velocity at the point of impact
        Dim vap1 As Vector = ca.Velocity + Vector.Cross(ca.AngularVelocity, rap)
        Dim vbp1 As Vector = cb.Velocity + Vector.Cross(cb.AngularVelocity, rbp)

        ' Relative velocity (the velocity at which the colliding points approach each other)
        Dim vab1 As Vector = vap1 - vbp1

        Dim num As Double = -(1 + e) * Vector.Dot(vab1, n)
        Dim div As Double = CSng(1 / ca.Mass + 1 / cb.Mass + Vector.Cross(rap, n) ^ 2 / ca.Inertia + Vector.Cross(rbp, n) ^ 2 / cb.Inertia)
        Dim j As Double = num / div

        ' Net impulse of the collision
        Dim impulse As Vector = j * n

        ' Velocity after the collision
        Dim va2 As Vector = ca.Velocity + impulse / ca.Mass
        Dim vb2 As Vector = cb.Velocity - impulse / cb.Mass

        ' Angular Velocity after the collision
        Dim wa2 As Double = ca.AngularVelocity + Vector.Cross(rap, impulse) / ca.Inertia
        Dim wb2 As Double = cb.AngularVelocity - Vector.Cross(rbp, impulse) / cb.Inertia

        If False Then
            ' Final collision velocity of the balls at the point of impact
            Dim vap2 As Vector = va2 + Vector.Cross(wa2, rap)
            Dim vbp2 As Vector = vb2 + Vector.Cross(wb2, rbp)
            Dim vab2 As Vector = vap2 - vbp2

            If Math.Abs(Vector.Dot(vab2, n) - -e * Vector.Dot(vab1, n)) > 0.1 Then
                ' Something went wrong and the calculation is incorrect
                Stop
            End If
        End If

        Return New CollisionResult(va2, vb2, wa2, wb2)
    End Function

    Private Function TestForCollision(c1 As Circle, c2 As Circle) As Double
        Dim separation As Double = c1.Radius + c2.Radius
        Dim da As Vector = New Vector(c1.Location, c2.Location)
        If da.Magnitude <= separation Then Return da.Magnitude - separation

        Return 0
    End Function

    Private Function DoWallCollision(c As Circle, eleapsedTime As Double) As Boolean
        Dim col As Double
        Dim impactVector As Vector = c.ImpactVector
        Dim wall As Circle = New Circle(c)
        wall.Velocity.Magnitude = wall.Velocity.Magnitude / 2 - friction

        ' Test against left wall
        wall.Velocity = New Vector(wall.Velocity.Magnitude, 0, 0 - wall.Radius, c.Y)
        col = TestForCollision(c, wall)
        If col <> 0 Then
            c.Velocity.Move(col)
            c.SetVelocity(DoCollision(c, wall).Circle1FinalVelocity, eleapsedTime)
            Return True
        End If

        ' Test against right wall
        wall.Velocity = New Vector(wall.Velocity.Magnitude, 180, mBounds.Width + wall.Radius, c.Y)
        impactVector.Angle = wall.Velocity.Angle + 180
        If impactVector.X2 >= mBounds.Width Then
            c.SetVelocity(DoCollision(c, wall).Circle1FinalVelocity, eleapsedTime)
            Return True
        End If

        ' Test against bottom wall
        wall.Velocity = New Vector(wall.Velocity.Magnitude, 90, c.X, 0 - wall.Radius)
        impactVector.Angle = wall.Velocity.Angle + 180
        If impactVector.Y2 <= 0 Then
            c.SetVelocity(DoCollision(c, wall).Circle1FinalVelocity, eleapsedTime)
            Return True
        End If

        ' Test against top wall
        wall.Velocity = New Vector(wall.Velocity.Magnitude, 270, c.X, mBounds.Height - wall.Radius)
        impactVector.Angle = wall.Velocity.Angle + 180
        If impactVector.Y2 >= mBounds.Height Then
            c.SetVelocity(DoCollision(c, wall).Circle1FinalVelocity, eleapsedTime)
            Return True
        End If

        Return False
    End Function

    Public Function GetCircleAt(x As Double, y As Double) As Circle
        For Each c As Circle In mCircles
            If Vector.Distance(x, y, c.X, c.Y) <= c.Diameter Then Return c
        Next

        Return Nothing
    End Function

    Public ReadOnly Property Sum() As Circle
        Get
            Dim x As Double
            Dim y As Double
            Dim nc As Circle = Nothing
            For Each c As Circle In mCircles
                If nc Is Nothing Then
                    nc = New Circle(c)
                Else
                    nc.Mass += c.Mass
                    nc.Velocity += c.Velocity
                End If
                x += c.X
                y += c.Y
            Next

            x /= mCircles.Count
            y /= mCircles.Count
            nc.Location = New PointF(x, y)

            Return nc
        End Get
    End Property

    Public Sub Add(item As Circle) Implements ICollection(Of Circle).Add
        AddHandler item.PositionChanged, Sub(sender As Circle)
                                             RaiseEvent CirclePositionChanged(sender)
                                         End Sub
        mCircles.Add(item)
    End Sub

    Public Sub AddCollection(cc As CirclesCollection)
        For Each c As Circle In cc
            Me.Add(c)
        Next
    End Sub

    Public Sub AddWithNumber(item As Circle, Number As Integer)
        Me.Add(item)
        mCircles(mCircles.Count - 1).Number = Number
    End Sub

    Public Sub Clear() Implements ICollection(Of Circle).Clear
        mCircles.Clear()
    End Sub

    Public Function Contains(item As Circle) As Boolean Implements ICollection(Of Circle).Contains
        For Each c As Circle In mCircles
            If c = item Then Return True
        Next

        Return False
    End Function

    Public Sub CopyTo(array() As Circle, arrayIndex As Integer) Implements ICollection(Of Circle).CopyTo
        mCircles.CopyTo(array, arrayIndex)
    End Sub

    Public ReadOnly Property Count() As Integer Implements ICollection(Of Circle).Count
        Get
            Return mCircles.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly() As Boolean Implements ICollection(Of Circle).IsReadOnly
        Get
            Return mCircles.IsReadOnly
        End Get
    End Property

    Public Function Remove(item As Circle) As Boolean Implements ICollection(Of Circle).Remove
        mCircles.Remove(item)
    End Function

    Public Sub QueRemove(item As Circle)
        queRemoveCircles.Add(item)
    End Sub

    Public Function GetEnumerator() As IEnumerator(Of Circle) Implements IEnumerable(Of Circle).GetEnumerator
        Return mCircles.GetEnumerator
    End Function

    Public Function IndexOf(item As Circle) As Integer Implements IList(Of Circle).IndexOf
        Return mCircles.IndexOf(item)
    End Function

    Public Sub Insert(index As Integer, item As Circle) Implements IList(Of Circle).Insert
        mCircles.Insert(index, item)
    End Sub

    Default Public Property Item(index As Integer) As Circle Implements IList(Of Circle).Item
        Get
            Return mCircles.Item(index)
        End Get
        Set(value As Circle)
            mCircles.Item(index) = value
        End Set
    End Property

    Public Sub RemoveAt(index As Integer) Implements IList(Of Circle).RemoveAt
        mCircles.RemoveAt(index)
    End Sub

    Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Return mCircles.GetEnumerator
    End Function

#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free unmanaged resources when explicitly called
            End If

            ' TODO: free shared unmanaged resources
        End If

        If simulationThread IsNot Nothing Then simulationThread.Abort()
        mCircles.Clear()
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
