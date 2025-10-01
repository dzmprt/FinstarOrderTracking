export const formatDateTime = (dateString: string): string => {
  try {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) {
      return 'Invalid Date';
    }
    return date.toLocaleString();
  } catch {
    return 'Invalid Date';
  }
};

export const formatDate = (dateString: string): string => {
  try {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) {
      return 'Invalid Date';
    }
    return date.toLocaleDateString();
  } catch {
    return 'Invalid Date';
  }
};
