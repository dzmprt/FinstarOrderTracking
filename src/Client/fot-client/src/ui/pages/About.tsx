
export const About: React.FC = () => (
  <div className="container my-4">
    <div className="card shadow-sm border-0">
      <div className="card-body">
        <h1 className="h4 mb-3">Finstar Order Tracking</h1>
        <p className="mb-2">
          <strong>Professional fullstack order tracking system for Finstar.</strong>
        </p>
        <p>
          <a href="https://github.com/dzmprt/FinstarOrderTracking" target="_blank" rel="noopener noreferrer" className="fw-bold">
            Project GitHub Repository
          </a>
        </p>
        <section aria-labelledby="features-heading">
          <h2 id="features-heading" className="h5 mt-4 mb-2">Key Features</h2>
          <ul className="mb-3">
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
        </section>
      </div>
    </div>
  </div>
);


