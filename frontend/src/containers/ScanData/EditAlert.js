import React from 'react';
import Dialog, {
    DialogActions,
    DialogContent,
    DialogTitle,
    DialogContentText
} from 'components/uielements/dialogs';
import Button from 'components/uielements/button';
import { CircularProgress } from 'components/uielements/progress';

class Alert extends React.Component {
    render() {
        const { status, loading, onSubmit, onClose } = this.props;
        if (loading) {
            return <CircularProgress />
        }
        return (
            <Dialog open={status} onClose={() => onClose(false)}>
                <DialogTitle>{"Edit Scan Data"}</DialogTitle>
                <DialogContent>
                    <DialogContentText>You cannot edit multiple scan data</DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => onClose(false)} color="primary">
                        Close
                    </Button>
                </DialogActions>
            </Dialog>
        )
    }
}

export default Alert;