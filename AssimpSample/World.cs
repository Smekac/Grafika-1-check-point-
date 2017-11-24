// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;

namespace AssimpSample
{

    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
   
        private AssimpScene m_scene;
        private AssimpScene m_scene2;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        private float m_zRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 195.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

       

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_scene; }
            set { m_scene = value; }
        }

        public AssimpScene Scene2
        {
            get { return m_scene2; }
            set { m_scene2 = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        public float RotationZ
        {
            get { return m_zRotation; }
            set { m_zRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String sceneFileName, String sceneFileName2, int width, int height, OpenGL gl)
        {
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_scene2 = new AssimpScene(scenePath, sceneFileName2, gl);           // !!!!!!!!!!!!!!!!!!!!!!
            this.m_width = width;
            this.m_height = height;
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 0f, 0f);
            gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            m_scene.LoadScene();
            m_scene.Initialize();
            m_scene2.LoadScene();
            m_scene2.Initialize();
        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.LoadIdentity();
            gl.PushMatrix();
            gl.Translate(-60f, -100f, -m_sceneDistance);
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);
            gl.Rotate(m_zRotation, 0.0f, 0.0f, 1.0f);

            gl.Viewport(0, 0, m_width, m_height);
            // gl.Scale(10f, 10f, 10f);
            //gl.PopMatrix();
                
            gl.PushMatrix();                //pROBAA sa gridom
            gl.Translate(0f,100f,0f);
            gl.Scale(92f,72f,0f);
            Grid mreza = new Grid();
            mreza.Render(gl,RenderMode.Design);
            gl.PopMatrix();
               
            //gl.PushMatrix();        //Ima na pocetku push
            gl.Rotate(-90f, 0f, 180f);                      //iscrtavanje modela
            m_scene2.Draw();
            gl.Color(1f, 0f, 0f);


            gl.Translate(-60f, -20f, 260f);
            m_scene.Draw();

            // gl.Scale(-10f, -10f, -10f);


            gl.Rotate(-90f, 0f, 0f);
            Cube cube = new Cube();                         //prepreke
            gl.Color(1f, 0.4f, 0.1f);
            // gl.Translate(-130f, -6000f, 0f);
            gl.Translate(-130f, -580f, 0f);
            gl.Scale(4f, 4f, 4f);
            for (int i = 0; i < 62; i++)
            {
                cube.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
                gl.Translate(0.0f, 6.0f, 0f);
            }

            gl.Translate(80f, -372.0f, 0.0f);
            for (int i = 0; i < 62; i++)
            {
                cube.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
                gl.Translate(0.0f, 6f, 0f);
            }

            gl.Color(0.4f, 0.4f, 0.4f);
            gl.Translate(0f, -480f, -1f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(200f, 100f);
            gl.Vertex(200f, 480f);
            gl.Vertex(-250f, 480f);
            gl.Vertex(-250f, 100f);
            gl.End();

            gl.Translate(0f, 0f, 0.1f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.7f, 0.7f, 0.7f);
            gl.Vertex(0f, 100f);
            gl.Vertex(0f, 480f);
            gl.Vertex(-80f, 480f);
            gl.Vertex(-80f, 100f);
            gl.End();

            gl.Viewport(m_width * 3 / 4, 0, m_width / 2, m_height / 2);   // Predefinisan ViewPort ....
            gl.Color(0f, 1f, 0f);
            gl.DrawText(10, 214, 0.5f, 1f, 1f, "Arial", 14, "Predmet:Racunarska grafika");
            gl.DrawText(10, 174, 0.5f, 1f, 1f, "Arial", 14, "Sk.god: 2017/18");
            gl.DrawText(10, 134, 0.5f, 1f, 1f, "Arial", 14, "Ime: Dejan");        
            gl.DrawText(10, 94, 0.5f, 1f, 1f, "Arial", 14, "Prezme: Stojkic");
            gl.DrawText(10, 54, 0.5f, 1f, 1f, "Arial", 14, "Sifra zad: 2.1");       
        
            gl.PopMatrix();
            gl.Flush();
        }




        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.Viewport(0, 0, width, height);
            gl.Perspective(50f, (double)width / height, 1f, 20000f);
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();			              // resetuj Projection Matrix

            if (m_width <= m_height)
            {
                gl.Frustum(-400.0f, 400.0f, -400.0f * m_height / m_width, 400.0f * m_height / m_width, 100.0f, 1000.0f);
            }
            else
            {
                gl.Frustum(-400.0f * m_width / m_height, 400.0f * m_width / m_height, -400.0f, 400.0f, 100.0f, 1000.0f);
            }

            gl.MatrixMode(OpenGL.GL_MODELVIEW);   // selektuj ModelView Matrix
            gl.LoadIdentity();                // resetuj ModelView Matrix
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene.Dispose();
                m_scene2.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}