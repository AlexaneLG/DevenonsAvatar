// Copyright (c) 2010 Bob Berkebile
// Please direct any bugs/comments/suggestions to http://www.pixelplacement.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Pixelplacement/iTweenPath")]
public class iTweenPathLocal : iTweenPath
{
    [HideInInspector]
	public List<Vector3> nodesWorld = new List<Vector3>();
    
	public override void OnDrawGizmosSelected(){
		if(pathVisible){
			if(nodes.Count > 0){
                UpdateWorldNodes();
                iTween.DrawPath(nodesWorld.ToArray(), pathColor);
			}	
		}
	}

    public void Start()
    {
        UpdateWorldNodes();
    }

    public void Update()
    {
        UpdateWorldNodes();
    }

    public override void CenterOnNodes()
    {
        UpdateWorldNodes();

        Vector3 Center = Vector3.zero;

        for (int i = 0; i < nodes.Count; i++)
        {
            Center += nodesWorld[i];
        }

        Center /= nodes.Count;

        Vector3 offset = transform.position - Center;

        transform.position = Center;

        Debug.Log(Center);


        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i] -= offset;
        }
  
    }

    public void UpdateWorldNodes()
    {
       if(nodesWorld.Count != nodes.Count){
           nodesWorld = new List<Vector3>(nodes);
       }

       for (int i = 0; i < nodes.Count;i++ )
       {
           nodesWorld[i] = transform.TransformPoint(nodes[i]);
       }

    }

	
	/// <summary>
	/// Returns the visually edited path as a Vector3 array.
	/// </summary>
	/// <param name="requestedName">
	/// A <see cref="System.String"/> the requested name of a path.
	/// </param>
	/// <returns>
	/// A <see cref="Vector3[]"/>
	/// </returns>
	public static Vector3[] GetWorldPath(string requestedName){
        requestedName = requestedName.ToLower();
		if(paths.ContainsKey(requestedName)){
            return (paths[requestedName] as iTweenPathLocal).nodesWorld.ToArray();
		}else{
			Debug.Log("No path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
			return null;
		}
	}
	
	/// <summary>
	/// Returns the reversed visually edited path as a Vector3 array.
	/// </summary>
	/// <param name="requestedName">
	/// A <see cref="System.String"/> the requested name of a path.
	/// </param>
	/// <returns>
	/// A <see cref="Vector3[]"/>
	/// </returns>
	public static Vector3[] GetWorldPathReversed(string requestedName){
		requestedName = requestedName.ToLower();
		if(paths.ContainsKey(requestedName)){
            List<Vector3> revNodes = (paths[requestedName] as iTweenPathLocal).nodesWorld.GetRange(0, paths[requestedName].nodes.Count);
			revNodes.Reverse();
			return revNodes.ToArray();
		}else{
			Debug.Log("No path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
			return null;
		}
	}
}