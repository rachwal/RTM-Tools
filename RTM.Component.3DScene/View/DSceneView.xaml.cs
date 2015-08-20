// RTM.Tools
// RTM.Component.3DScene
// DSceneView.xaml.cs
// 
// Created by Bartosz Rachwal. 
// Copyright (c) 2015 Bartosz Rachwal. The National Institute of Advanced Industrial Science and Technology, Japan. All rights reserved. 

using RTM.Component._3DScene.ViewModel;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;

namespace RTM.Component._3DScene
{
    public partial class DSceneView
    {
        private ISceneViewModel ViewModel
        {
            get { return (ISceneViewModel) DataContext; }
            set { DataContext = value; }
        }

        public DSceneView(ISceneViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();
        }

        private void OpenGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;

            gl.Enable(OpenGL.GL_DEPTH_TEST);

            var globalAmbient = new[] {0.5f, 0.5f, 0.5f, 1.0f};
            var light0Pos = new[] {0.0f, 5.0f, 10.0f, 1.0f};
            var light0Ambient = new[] {0.2f, 0.2f, 0.2f, 1.0f};
            var light0Diffuse = new[] {0.3f, 0.3f, 0.3f, 1.0f};
            var light0Specular = new[] {0.8f, 0.8f, 0.8f, 1.0f};
            var lmodelAmbient = new[] {0.2f, 0.2f, 0.2f, 1.0f};

            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, lmodelAmbient);

            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, globalAmbient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0Pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0Ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0Diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light0Specular);
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);

            gl.ShadeModel(OpenGL.GL_SMOOTH);

            var tp = new Teapot();
            tp.Draw(gl, 14, 2, OpenGL.GL_FILL);
        }
    }
}