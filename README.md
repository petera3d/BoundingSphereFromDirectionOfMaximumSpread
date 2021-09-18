# BoundingSphereFromDirectionOfMaximumSpread

Create Bounding Sphere from 3d points using Covariance matrix and Jacobi rotation matrix algorithm to obtain eigenvalues and eigenvectors from synmetrix matrix.

This code is implemented in Unity and C#.
I use it in my games in various situations, for example when I need to compute the force, direction and spread of weapon power  along the enemies.

To compute bounding sphere we need the following steps 

1) Create random 2d/3d points

2) Compute covariance matrix to get the maximum spread. Covariance matrices are always symmetric

3)Run Jacobi rotation algorithm on symmetric covariance matrix to get Eigenvalues and Eigenvectors.
the largest eigenvector (maximum spread) will show us points spread direction.

4)Compute minimum and maximum extreme points along largest spread direction of the points.

5)Compute sphere center and radius using extreme points.

6)In the last step run function that iterate thought all points and checks if points is within or out of bounding sphere. 
If point is not encompassed by sphere increase the sphere radius and displace the sphere center.



BOOKS

Golub VanLoan Matrix Computations 4th (Section 8.5)

Christer Ericson Real-Time Collision Detection p.90
