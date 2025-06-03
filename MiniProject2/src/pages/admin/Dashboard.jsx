import { Link, Outlet } from "react-router-dom";

function Dashboard() {
  return (
    <div className="admin-dashboard">
      <div className="page">
        <div className="hero" style={{ transition: "all 0.2s ease-in-out" }}>
          <h1 className="hero-title">ADMIN DASHBOARD</h1>
        </div>
        <div className="container">
          <ul className="admin-options">
            <li style={{ marginRight: "20px" }}>
              <Link to="UserList">View all Users</Link>
            </li>
            <li>
              <Link to="PropertyList">View All Properties</Link>
            </li>
            <li>
              <Link to="/">Return to Home</Link>
            </li>
          </ul>
          <Outlet />
          {/* https://api.reactrouter.com/v7/functions/react_router.Outlet.html */}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
