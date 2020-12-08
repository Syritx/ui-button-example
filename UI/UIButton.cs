using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;

namespace _3d.UI {

    /// <summary>
    /// his class performs an important function.
    /// </summary>
    class UIButton : UIComponent {

        Vector2 dimensions;
        float[] vertices, t;    
        Shader shader;
        Camera camera;
        Vector2[] cornerPositions;

        int vao, vbo;
        int id = 1;
        bool isHovered = false;
        Action ClickMethod;

        /// <summary>
        /// this class creates an interactable UI button that can call a set method
        /// </summary>
        public UIButton(string vertexShaderPath, string fragmentShaderPath, Vector2 position, Vector2 offset, Vector2 dimensions, Camera camera, Action ClickMethod) {
            shader = new Shader(vertexShaderPath, fragmentShaderPath);
            this.dimensions = dimensions;
            this.camera = camera;
            this.ClickMethod = ClickMethod;

            this.camera.game.MouseDown += OnMouseClick;

            float[] tempVertices = {
                position.X+((dimensions.X/2)+offset.X), position.Y+((dimensions.Y/2)+offset.Y), 0.0f,
                position.X+((dimensions.X/2)+offset.X), position.Y-((dimensions.Y/2)-offset.Y), 0.0f,
                position.X-((dimensions.X/2)+offset.X), position.Y-((dimensions.Y/2)-offset.Y), 0.0f,

                position.X-((dimensions.X/2)+offset.X), position.Y-((dimensions.Y/2)-offset.Y), 0.0f,
                position.X-((dimensions.X/2)+offset.X), position.Y+((dimensions.Y/2)+offset.Y), 0.0f,
                position.X+((dimensions.X/2)+offset.X), position.Y+((dimensions.Y/2)+offset.Y), 0.0f,
            };

            Vector2[] tempCornerPositions = {
                new Vector2(position.X+((dimensions.X/2)+offset.X), position.Y+((dimensions.Y/2)+offset.Y)), // top right
                new Vector2(position.X-((dimensions.X/2)-offset.X), position.Y+((dimensions.Y/2)+offset.Y)), // top left
                new Vector2(position.X+((dimensions.X/2)+offset.X), position.Y-((dimensions.Y/2)-offset.Y)), // bottom right
                new Vector2(position.X-((dimensions.X/2)-offset.X), position.Y-((dimensions.Y/2)-offset.Y)) // bottom left
            };
            cornerPositions = tempCornerPositions;

            t = tempVertices;
            vertices = tempVertices;

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(2,3,VertexAttribPointerType.Float, false, 0, 0);
        }

        public override void Render() {
            base.Render();
            shader.UseShader();

            if (camera.mousePosition2D.X <= cornerPositions[0].X && camera.mousePosition2D.X > cornerPositions[1].X 
             && camera.mousePosition2D.Y <= cornerPositions[1].Y && camera.mousePosition2D.Y > cornerPositions[2].Y) {
                 isHovered = true;
            }
            else isHovered = false;

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }

        void OnMouseClick(MouseButtonEventArgs e) {
            if (isHovered) {
                ClickMethod();
                System.Console.WriteLine("clicked button " + id);
            }
        }  
    }
}