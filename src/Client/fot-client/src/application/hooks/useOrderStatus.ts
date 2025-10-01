import { useState, useEffect } from 'react';
import { getNextStatusOptions, isValidStatusTransition } from '../utils/statusUtils';
import type { StatusOption } from '../types/ui';

export function useOrderStatus(currentStatus: number) {
  const [status, setStatus] = useState<string>('');
  const [statusOptions, setStatusOptions] = useState<StatusOption[]>([]);

  useEffect(() => {
    if (currentStatus) {
      const options = getNextStatusOptions(currentStatus);
      setStatusOptions(options);
      
      // Set default to first available option
      if (options.length > 0) {
        setStatus(String(options[0].value));
      }
    }
  }, [currentStatus]);

  const hasStatusChanged = isValidStatusTransition(currentStatus, Number(status));

  return {
    status,
    setStatus,
    statusOptions,
    hasStatusChanged,
  };
}
