import React from 'react';

interface PaginationProps {
  page: number;
  canPrev: boolean;
  canNext: boolean;
  setPage: (p: number) => void;
}

export const Pagination: React.FC<PaginationProps> = ({ page, canPrev, canNext, setPage }) => (
  <nav className="d-flex justify-content-between align-items-center" aria-label="Pagination">
    <ul className="pagination mb-0">
      <li className={`page-item ${!canPrev ? 'disabled' : ''}`}>
        <button className="page-link" onClick={() => setPage(Math.max(1, page - 1))}>Prev</button>
      </li>
      <li className="page-item disabled"><span className="page-link">Page {page}</span></li>
      <li className={`page-item ${!canNext ? 'disabled' : ''}`}>
        <button className="page-link" onClick={() => setPage(page + 1)}>Next</button>
      </li>
    </ul>
  </nav>
);
