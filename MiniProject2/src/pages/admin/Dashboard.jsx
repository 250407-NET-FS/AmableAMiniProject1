import { Link, Outlet } from "react-router-dom";

function Dashboard() {
  return (
    <div className="admin-dashboard">
      <div className="page">
        <div className="hero" style={{ transition: "all 0.2s ease-in-out" }}>
          
        </div>
        <div className="container">

          <Outlet />
          {/* https://api.reactrouter.com/v7/functions/react_router.Outlet.html */}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
