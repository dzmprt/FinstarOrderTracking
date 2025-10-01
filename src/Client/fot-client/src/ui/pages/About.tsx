export const About: React.FC = () => (
  <div className="container my-4">
    <div className="card shadow-sm border-0">
      <div className="card-body">
        <h1 className="h4 mb-3">About Finstar Order Tracking</h1>
        <p className="mb-2">
          This project is a test assignment for the Finstar company.<br/>
          <a href="https://github.com/dzmprt/FinstarOrderTracking"><h2>Project GitHub page</h2></a>
        </p>
        <p className="mb-0">
          <h4>Features</h4>
          <ul>
            <li>Order management: create, update, view, and filter orders</li>
            <li>Real-time order status updates via WebSocket</li>
            <li>RESTful API (ASP.NET Core WebApi)</li>
            <li>PostgreSQL database</li>
            <li>Kafka for event-driven architecture</li>
            <li>Outbox pattern for reliable event delivery</li>
            <li>Docker Compose for easy local development</li>
            <li>Nginx reverse proxy for unified API and frontend access</li>
            <li>Bootstrap UI for modern frontend design</li>
          </ul>
        </p>
      </div>
    </div>
  </div>
);


