using UnityEngine;
/// <summary>
/// 设置相机朝向对象，并设置最佳观察的视野
/// </summary>
public class LookToObject:MonoBehaviour{
	[Tooltip("相机所拍摄的对象")]
    public GameObject target;
    private MeshRenderer _targetMeshRenderer;
    private Camera _camera;

    private Vector3[] _points;
	private Vector3 _planeCenter;

    private void Start(){
        _targetMeshRenderer=target.GetComponent<MeshRenderer>();
        _camera=GetComponent<Camera>();
    }

    private void Update(){
        //相机旋转朝向目标对象
        Bounds bounds=_targetMeshRenderer.bounds;
		Vector3 boundsCenter=bounds.center;
        _camera.transform.LookAt(boundsCenter);
        //包围盒角点
        Vector3[] points=getBoundsCorners(boundsCenter,bounds.extents);
        //所有角点投射到平面
        Vector3 planeNormal=boundsCenter-_camera.transform.position;
        worldToPanelPoints(points,points.Length,planeNormal);
        _points=points;
        //平面中心
        Vector3 planeCenter=Vector3.ProjectOnPlane(boundsCenter,planeNormal);
		_planeCenter=planeCenter;
        //取平面上各个点与平面中心的最大距离作为相机的视野矩形框大小
        float halfHeight=getMaxDistanceToPlaneCenter(points,points.Length,planeCenter);
        //相机与包围盒中心的距离(世界坐标为单位)
        float distance=Vector3.Distance(boundsCenter,_camera.transform.position);
        //得到视野大小
        _camera.fieldOfView=Mathf.Atan2(halfHeight,distance)*Mathf.Rad2Deg*2;  
    }
    
    /// <summary>
	/// 返回包围盒的角点列表
	/// </summary>
	/// <param name="boundsCenter">包围盒的中心</param>
	/// <param name="boundsExtents">Bounds.extents</param>
	/// <returns></returns>
	private Vector3[] getBoundsCorners(Vector3 boundsCenter,Vector3 boundsExtents){
		Vector3[] vertices=new Vector3[8];
		//左下后
		vertices[0]=boundsCenter+Vector3.Scale(boundsExtents,new Vector3(-1,-1,-1));
		//左上后
		vertices[1]=boundsCenter+Vector3.Scale(boundsExtents,new Vector3(-1,1,-1));
		//右上后
		vertices[2]=boundsCenter+Vector3.Scale(boundsExtents,new Vector3(1,1,-1));
		//右下后
		vertices[3]=boundsCenter+Vector3.Scale(boundsExtents,new Vector3(1,-1,-1));

		//左下前
		vertices[4]=boundsCenter+Vector3.Scale(boundsExtents,new Vector3(-1,-1,1));
		//左上前
		vertices[5]=boundsCenter+Vector3.Scale(boundsExtents,new Vector3(-1,1,1));
		//右上前
		vertices[6]=boundsCenter+Vector3.Scale(boundsExtents,new Vector3(1,1,1));
		//右下前
		vertices[7]=boundsCenter+Vector3.Scale(boundsExtents,new Vector3(1,-1,1));
		return vertices;
	}
    
    /// <summary>
    /// 将世界坐标点数组投射到以原点为中心的平面
    /// </summary>
    /// <param name="points">世界坐标点数组</param>
    /// <param name="pointCount">坐标点数量</param>
    /// <param name="planeNormal">平面法线</param>
    /// <returns></returns>
    private Vector3[] worldToPanelPoints(Vector3[] points,int pointCount,Vector3 planeNormal){
        for(int i=0;i<pointCount;i++){
            var vertex=points[i];
            points[i]=Vector3.ProjectOnPlane(vertex,planeNormal);
        }
        return points;
    }
    
    /// <summary>
    /// 返回平面上各个点与平面中心的最大距离
    /// </summary>
    /// <param name="points">平面上的各个点</param>
    /// <param name="pointCount">点数量</param>
    /// <param name="planeCenter">平面中心</param>
    /// <returns></returns>
    private float getMaxDistanceToPlaneCenter(Vector3[] points,int pointCount,Vector3 planeCenter){
        float maxDistance=float.MinValue;
        for(int i=0;i<pointCount;i++){
            var vertex=points[i];
            float distance=Vector3.Distance(vertex,planeCenter);
            if(distance>maxDistance)maxDistance=distance;
        }
        return maxDistance;
    }
    
    private void OnDrawGizmos(){
        if(Application.isPlaying){
            if(_points!=null){
               drawPath(_points);
            }
			drawPoint(_planeCenter,0.02f);
        }
    }
    
    /// <summary>
	/// 根据路径点数组画线
	/// </summary>
	/// <param name="vertices">路径点数组</param>
	private void drawPath(Vector3[] vertices){
        int len=vertices.Length;
        for (int i=0;i<len;i++){
            int nexti=(i+1)%len;
            Gizmos.DrawLine(vertices[i],vertices[nexti]);
        }
    }

	/// <summary>
	/// 用一个球体线框画一个点
	/// </summary>
	/// <param name="point">点位置</param>
	/// <param name="radius">球体线框半径</param>
	private void drawPoint(Vector3 point,float radius=0.02f){
		Gizmos.DrawWireSphere(point,radius);
	}

    
}
