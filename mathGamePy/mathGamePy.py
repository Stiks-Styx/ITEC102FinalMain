
import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
from matplotlib.animation import FuncAnimation

# Define the triangle vertices in 3D
def create_triangle():
    # Vertices of the triangle base
    x = np.array([0, 1, 2, 3, 4, 5, 6, 1, 1, 2, 2, 3, 4, 5, 1, 2, 3, 2, 3, 4, 3])
    y = np.array([0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 4, 4, 5, 4, 4, 5])
    z = np.zeros_like(x)  # Start with a flat triangle (z = 0)
    return x, y, z

# Rotate the triangle
def rotate(x, y, z, angle):
    rotation_matrix = np.array([[np.cos(angle), -np.sin(angle), 0],
                                 [np.sin(angle), np.cos(angle), 0],
                                 [0, 0, 1]])
    coords = np.vstack((x, y, z))
    rotated_coords = rotation_matrix.dot(coords)
    return rotated_coords[0], rotated_coords[1], rotated_coords[2]

# Set up the figure and 3D axis
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

# Create triangle vertices
x, y, z = create_triangle()

# Set limits and labels
ax.set_xlim([-1, 8])
ax.set_ylim([-1, 8])
ax.set_zlim([-1, 1])
ax.set_xlabel('X axis')
ax.set_ylabel('Y axis')
ax.set_zlabel('Z axis')

# Animation function
def update(frame):
    ax.cla()  # Clear the current axes
    angle = frame * np.pi / 180  # Convert frame to radians
    x_rot, y_rot, z_rot = rotate(x, y, z, angle)  # Rotate triangle
    ax.plot_trisurf(x_rot, y_rot, z_rot, color='cyan', alpha=0.5)  # Plot rotated triangle
    ax.set_xlim([-1, 8])
    ax.set_ylim([-1, 8])
    ax.set_zlim([-1, 1])

# Create the animation
ani = FuncAnimation(fig, update, frames=360, interval=50)

plt.show()
