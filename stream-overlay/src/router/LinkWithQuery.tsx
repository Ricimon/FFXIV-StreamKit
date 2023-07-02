import { useLocation } from "react-router";
import { Link } from 'react-router-dom';

interface Props {
  children: React.ReactNode;
  to: string;
}

export default function LinkWithQuery(props: Props) {
  const { children, to, ...otherProps } = { ...props };
  const { search } = useLocation();

  return (
    <Link to={to + search} {...otherProps}>
      {children}
    </Link>
  )
}