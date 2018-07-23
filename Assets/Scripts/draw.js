 private var dragging : boolean = false;
 private var moving : boolean = false;
 private var countDrag : int = 0;
 private var countMove : int = 0;
 private var mousePosition : Vector3;
 private var mousePoint : Vector3;
 private var pointCurrent : Vector3;
 private var pointStore : Vector3;
 private var posStoreX : Array = [];
 private var posStoreZ : Array = [];
 private var arrayPathMarker : GameObject[] = new GameObject[100];
 var objectPathMarker : GameObject;
 
 function Start () {
     dragging = false;
     print("countDrag ");
 }
 
 function Update () {
     // note - raycast needs a surface to hit against     
     var rayHit : RaycastHit;
     if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), rayHit)) {
         // where did the raycast hit in the world - position of rayhit
         if (dragging) {print ("rayHit.point : " + rayHit.point + " (mousePoint)");}
         mousePoint = rayHit.point;
 
         if (Input.GetMouseButtonDown(0)) {
             OnTouchBegin(mousePoint);
         }
         else if (Input.GetMouseButton(0)) {
             OnTouchMove(mousePoint);
         }
         else if (Input.GetMouseButtonUp(0)) {
             OnTouchEnd(mousePoint);
         }
     }
 }
 
 function OnTouchBegin (pointCurrent : Vector3) {
     countDrag = 0; print("b ");
     posStoreX.Clear();
     posStoreZ.Clear();
     AddSplinePoint(pointCurrent);
     dragging = true;
     moving = false;
 }
 
 function OnTouchMove (pointCurrent : Vector3) {
     if ((dragging) && (countDrag < 100)) {
         print("countDrag " + countDrag);
         AddSplinePoint(pointCurrent);
     } else {
         dragging = false;
         moving = true;
     }
 }
 
 function OnTouchEnd (pointCurrent : Vector3) {
     dragging = false;
     moving = true;
 }
 
 function AddSplinePoint (pointStore : Vector3) {
     // store co-ordinates
     posStoreX[countDrag] = pointStore.x;
     posStoreZ[countDrag] = pointStore.z;
     
     // show path : Instantiate and load position into array as gameObject
     arrayPathMarker[countDrag] = Instantiate(objectPathMarker, Vector3(pointStore.x, 0, pointStore.z), transform.rotation);
     print (arrayPathMarker[countDrag].transform.position);
     
     // next position
     countDrag ++;
 }
 
 function FixedUpdate () {
     // move gameObject
     if (moving) {
         // remove path marker
         Destroy (arrayPathMarker[countMove]);
         
         // move gameObject along path
         transform.position = Vector3(posStoreX[countMove], 0, posStoreZ[countMove]);
         
         // next position
         countMove ++;
         if (countMove >= posStoreX.length) {moving = false;} // stop at end of path
     } else {
         countMove = 0;
     }
     
     if (Input.GetKeyDown("r")) {print ("posStoreX "+posStoreX+" : posStoreZ "+posStoreZ);}
 }