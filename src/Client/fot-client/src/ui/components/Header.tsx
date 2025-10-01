import { NavLink } from 'react-router-dom';
import { useAppSelector } from '../../application/hooks';
import { WEBSOCKET_STATUS } from '../../application/constants/websocket';

export const Header: React.FC = () => {
  const { isConnected } = useAppSelector(s => s.websocket);

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
      <div className="container-fluid">
        <NavLink to="/orders" className="navbar-brand fw-semibold text-white">Finstar Orders</NavLink>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mainNav" aria-controls="mainNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="mainNav">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <NavLink to="/about" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>About</NavLink>
            </li>
            <li className="nav-item">
              <NavLink to="/orders" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>Orders</NavLink>
            </li>
            <li className="nav-item">
              <NavLink to="/websocket" className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}>Live Orders</NavLink>
            </li>
          </ul>
          <div className="d-flex align-items-center">
            <span className={`badge ${isConnected ? 'text-bg-success' : 'text-bg-danger'}`}>
              <span className="d-none d-sm-inline">WebSocket </span>
              {isConnected ? WEBSOCKET_STATUS.CONNECTED : WEBSOCKET_STATUS.DISCONNECTED}
            </span>
          </div>
        </div>
      </div>
    </nav>
  );
};


